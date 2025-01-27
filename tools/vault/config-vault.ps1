$newlines = "`n"

# --- ASCII Art for CodeDesignPlus ---
$asciiArt = @"
   ___          _         ___          _               ___ _           
  / __\___   __| | ___   /   \___  ___(_) __ _ _ __   / _ \ |_   _ ___ 
 / /  / _ \ / _` |/ _ \ / /\ / _ \/ __| |/ _` | '_ \ / /_)/ | | | / __|
/ /__| (_) | (_| |  __// /_//  __/\__ \ | (_| | | | / ___/| | |_| \__ \
\____/\___/ \__,_|\___/___,' \___||___/_|\__, |_| |_\/    |_|\__,_|___/
                                         |___/                         
"@
Write-Host $asciiArt

# Set Vault Address
$env:VAULT_ADDR = "http://localhost:8200"

# Vault Login
Write-Host "-Logging in to Vault..."  -ForegroundColor Blue
vault login token=root

# --- 1. Enabling Auth Methods ---
Write-Host $newlines
Write-Host "1. Enabling auth methods..." -ForegroundColor Blue
if (vault auth list | Select-String -Pattern 'approle/' -Quiet) {
    Write-Host "  - The method of authentication 'approle' already exists."
}
else {
    Write-Host "  - Enabling the method of authentication 'approle'..."
    vault auth enable approle
}

# --- 2. Enabling Secret Engines ---
Write-Host $newlines
Write-Host "2. Enabling secrets engines..." -ForegroundColor Blue

if (vault secrets list | Select-String -Pattern 'vault-keyvalue/' -Quiet) {
    Write-Host "  - The secrets engine 'kv-v2' already exists in 'vault-keyvalue/'."
}
else {
    Write-Host "  - Enabling the secrets engine 'kv-v2' in 'vault-keyvalue/'..."
    vault secrets enable -path=vault-keyvalue kv-v2
}

if (vault secrets list | Select-String -Pattern 'vault-database/' -Quiet) {
    Write-Host "  - The secrets engine 'database' already exists in 'vault-database/'."
}
else {
    Write-Host "  - Enabling the secrets engine 'database' in 'vault-database/'..."
    vault secrets enable -path=vault-database database
}

if (vault secrets list | Select-String -Pattern 'vault-rabbitmq/' -Quiet) {
    Write-Host "  - The secrets engine 'rabbitmq' already exists in 'vault-rabbitmq/'."
}
else {
    Write-Host "  - Enabling the secrets engine 'rabbitmq' in 'vault-rabbitmq/'..."
    vault secrets enable -path=vault-rabbitmq rabbitmq
}

if (vault secrets list | Select-String -Pattern 'vault-transit/' -Quiet) {
    Write-Host "  - The secrets engine 'transit' already exists in 'vault-transit/'."
}
else {
    Write-Host "  - Enabling the secrets engine 'transit' in 'vault-transit/'..."
    vault secrets enable -path=vault-transit transit
}

# --- 3. Applying Policies ---
Write-Host $newlines
Write-Host "3. Applying policies..." -ForegroundColor Blue
$policyName = "full-access"
vault policy read $policyName
if ($?) {
    Write-Host "  - The policy '$policyName' already exists."
}
else {
    Write-Host "  - Creating policy '$policyName'..."
    $policyContent = @"
path "*" {
  capabilities = ["create", "read", "update", "delete", "list"]
}
"@
    $policyContent | vault policy write $policyName -
}

# --- 4. Creating Roles ---
Write-Host $newlines
Write-Host "4. Creating roles..." -ForegroundColor Blue
$roleName = "vault-approle"
vault read auth/approle/role/$roleName
if ($?) {
    Write-Host "  - The AppRole '$roleName' already exists."
}
else {
    Write-Host "  - Creating AppRole '$roleName'..."
    vault write auth/approle/role/$roleName policies="full-access"
}

# Get role_id
$role_id_output = vault read auth/approle/role/vault-approle/role-id
$role_id_match = $role_id_output | Select-String -Pattern 'role_id\s+([\w-]+)'
if ($role_id_match) {
    $role_id = $role_id_match.Matches[0].Groups[1].Value
}
else {
    Write-Error "Error: Could not find role_id"
    exit 1
}

# Get secret_id
$secret_id_output = vault write -f auth/approle/role/vault-approle/secret-id
$secret_id_match = $secret_id_output | Select-String -Pattern 'secret_id\s+([\w-]+)'
if ($secret_id_match) {
    $secret_id = $secret_id_match.Matches[0].Groups[1].Value
}
else {
    Write-Error "Error: Could not find secret_id"
    exit 1
}

if (-not $role_id -or -not $secret_id) {
    Write-Error "Error: Not found role_id or secret_id"
    exit 1
}

Write-Host "  Role ID: $role_id"
Write-Host "  Secret ID: $secret_id"

# --- 5. Login with approle ---
Write-Host $newlines
Write-Host "5. Login with approle..." -ForegroundColor Blue
vault write auth/approle/login role_id=$role_id secret_id=$secret_id

# --- 6. Writing secrets ---
Write-Host $newlines
Write-Host "6. Writing secrets..." -ForegroundColor Blue
vault kv put -mount=vault-keyvalue ms-archetype `
    Security:ClientId=a74cb192-598c-4757-95ae-b315793bbbca `
    Security:ValidAudiences:0=a74cb192-598c-4757-95ae-b315793bbbca `
    Security:ValidAudiences:1=api://a74cb192-598c-4757-95ae-b315793bbbca

vault kv get -mount=vault-keyvalue ms-archetype

# --- 7. Writing database configuration ---
Write-Host $newlines
Write-Host "7. Writing database configuration..." -ForegroundColor Blue
vault write vault-database/config/db-ms-archetype `
    plugin_name=mongodb-database-plugin `
    allowed_roles="ms-archetype-mongo-role" `
    connection_url="mongodb://{{username}}:{{password}}@mongo:27017/admin?ssl=false" `
    username="admin" `
    password="password"

vault write vault-database/roles/ms-archetype-mongo-role `
    db_name=db-ms-archetype `
    creation_statements="{ """db""": """admin""", """roles""": [{ """role""": """readWrite""", """db""": """db-ms-archetype""" }] }" `
    default_ttl="1h" `
    max_ttl="24h"

vault read vault-database/creds/ms-archetype-mongo-role

# --- 8. Writing rabbitmq configuration ---
Write-Host $newlines
Write-Host "8. Writing rabbitmq configuration..." -ForegroundColor Blue

Write-Host "Waiting for RabbitMQ to start..."
Start-Sleep -Seconds 12

vault write vault-rabbitmq/config/connection `
    connection_uri="http://rabbitmq:15672" `
    username="admin" `
    password="password"

vault write vault-rabbitmq/roles/ms-archetype-rabbitmq-role `
    vhosts="{"""/""":{"""write""": """.*""", """read""": """.*""", """configure""": """.*"""}}"

vault read vault-rabbitmq/creds/ms-archetype-rabbitmq-role