$newlines = "`n" * 2 + "----------------------------------------"

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
Write-Host "-. Logging in to Vault..."
vault login token=root

# --- 1. Enabling Auth Methods ---
Write-Host $newlines
Write-Host "1. Enabling auth methods..."
vault auth enable approle

# --- 2. Enabling Secret Engines ---
Write-Host $newlines
Write-Host "2. Enabling secrets engines..."
vault secrets enable -path=archetype-keyvalue kv-v2
vault secrets enable -path=archetype-database database
vault secrets enable -path=archetype-rabbitmq rabbitmq
vault secrets enable -path=archetype-transit transit

# --- 3. Applying Policies ---
Write-Host $newlines
Write-Host "3. Applying policies..."
$policyContent = @"
path "*" {
  capabilities = ["create", "read", "update", "delete", "list"]
}
"@

$policyContent | vault policy write full-access -

# --- 4. Creating Roles ---
Write-Host $newlines
Write-Host "4. Creating roles..."
vault write auth/approle/role/archetype-approle policies="full-access"

# Get role_id
$role_id_output = vault read auth/approle/role/archetype-approle/role-id
$role_id_match = $role_id_output | Select-String -Pattern 'role_id\s+([\w-]+)'
if ($role_id_match) {
    $role_id = $role_id_match.Matches[0].Groups[1].Value
} else {
    Write-Error "Error: Could not find role_id"
    exit 1
}


# Get secret_id
$secret_id_output = vault write -f auth/approle/role/archetype-approle/secret-id
$secret_id_match = $secret_id_output | Select-String -Pattern 'secret_id\s+([\w-]+)'
if ($secret_id_match) {
    $secret_id = $secret_id_match.Matches[0].Groups[1].Value
} else {
    Write-Error "Error: Could not find secret_id"
    exit 1
}

if (-not $role_id -or -not $secret_id) {
    Write-Error "Error: Not found role_id or secret_id"
    exit 1
}

Write-Host "Role ID: $role_id"
Write-Host "Secret ID: $secret_id"

# --- 5. Login with approle ---
Write-Host $newlines
Write-Host "5. Login with approle..."
vault write auth/approle/login role_id=$role_id secret_id=$secret_id

# --- 6. Writing secrets ---
Write-Host $newlines
Write-Host "6. Writing secrets..."
vault kv put -mount=archetype-keyvalue ms-archetype `
    Security:ClientId=a74cb192-598c-4757-95ae-b315793bbbca `
    Security:ValidAudiences:0=a74cb192-598c-4757-95ae-b315793bbbca `
    Security:ValidAudiences:1=api://a74cb192-598c-4757-95ae-b315793bbbca `
    Redis:Instances:Core:ConnectionString=localhost:6379

vault kv get -mount=archetype-keyvalue ms-archetype

# --- 7. Writing database configuration ---
Write-Host $newlines
Write-Host "7. Writing database configuration..."
vault write archetype-database/config/db-ms-archetype `
    plugin_name=mongodb-database-plugin `
    allowed_roles="ms-archetype-mongo-role" `
    connection_url="mongodb://{{username}}:{{password}}@mongo:27017/admin?ssl=false" `
    username="admin" `
    password="password"

vault write archetype-database/roles/ms-archetype-mongo-role `
    db_name=db-ms-archetype `
    creation_statements="{ \""db\"": \""admin\"", \""roles\"": [{ \""role\"": \""readWrite\"", \""db\"": \""db-ms-archetype\"" }] }" `
    default_ttl="1h" `
    max_ttl="24h"

vault read archetype-database/creds/ms-archetype-mongo-role

# --- 8. Writing rabbitmq configuration ---
Write-Host $newlines
Write-Host "8. Writing rabbitmq configuration..."

Write-Host "Waiting for RabbitMQ to start..."
Start-Sleep -Seconds 12

vault write archetype-rabbitmq/config/connection `
    connection_uri="http://rabbitmq:15672" `
    username="admin" `
    password="password"

vault write archetype-rabbitmq/roles/ms-archetype-rabbitmq-role `
    vhosts="{\""/\"":{\""write\"": \"".*\"", \""read\"": \"".*\"", \""configure\"": \"".*\""}}"

vault read archetype-rabbitmq/creds/ms-archetype-rabbitmq-role