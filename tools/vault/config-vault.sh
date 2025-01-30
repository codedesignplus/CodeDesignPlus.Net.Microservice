#!/bin/bash

# Define colors
BLUE='\033[0;34m'
NC='\033[0m' # No color

# ASCII Art for CodeDesignPlus
ascii_art=$(cat <<'EOF'
   ___          _         ___          _               ___ _           
  / __\___   __| | ___   /   \___  ___(_) __ _ _ __   / _ \ |_   _ ___ 
 / /  / _ \ / _` |/ _ \ / /\ / _ \/ __| |/ _` | '_ \ / /_)/ | | | / __|
/ /__| (_) | (_| |  __// /_//  __/\__ \ | (_| | | | / ___/| | |_| \__ \
\____/\___/ \__,_|\___/___,' \___||___/_|\__, |_| |_\/    |_|\__,_|___/
                                         |___/                         
EOF
)
echo "$ascii_art"

# Your Vault Address and Login
export VAULT_ADDR="http://localhost:8200"
echo "${BLUE}-Logging in to Vault...${NC}"
vault login token=root

newlines=$'\n'

# Enable AppRole
echo "$newlines"
echo "${BLUE}1. Enabling auth methods...${NC}"
if vault auth list | grep -q 'approle/'; then
    echo "  - The method of authentication 'approle' already exists."
else
    echo "  - Enabling the method of authentication 'approle'..."
    vault auth enable approle
fi

# Enable Secret, database and rabbtimq
echo "$newlines"
echo "${BLUE}2. Enabling secrets engines...${NC}"
if vault secrets list | grep -q 'vault-keyvalue/'; then
    echo "  - The secrets engine 'kv-v2' already exists in 'vault-keyvalue/'."
else
    echo "  - Enabling the secrets engine 'kv-v2' in 'vault-keyvalue/'..."
    vault secrets enable -path=vault-keyvalue kv-v2
fi

if vault secrets list | grep -q 'vault-database/'; then
    echo "  - The secrets engine 'database' already exists in 'vault-database/'."
else
    echo "  - Enabling the secrets engine 'database' in 'vault-database/'..."
    vault secrets enable -path=vault-database database
fi

if vault secrets list | grep -q 'vault-rabbitmq/'; then
    echo "  - The secrets engine 'rabbitmq' already exists in 'vault-rabbitmq/'."
else
    echo "  - Enabling the secrets engine 'rabbitmq' in 'vault-rabbitmq/'..."
    vault secrets enable -path=vault-rabbitmq rabbitmq
fi

if vault secrets list | grep -q 'vault-transit/'; then
    echo "  - The secrets engine 'transit' already exists in 'vault-transit/'."
else
    echo "  - Enabling the secrets engine 'transit' in 'vault-transit/'..."
    vault secrets enable -path=vault-transit transit
fi

# Create policies
echo "$newlines"
policy_name="full-access"
if vault policy read $policy_name > /dev/null 2>&1; then
    echo "${BLUE}3. The policy '$policy_name' already exists.${NC}"
else
    echo "${BLUE}3. Creating policy '$policy_name'...${NC}"
    vault policy write $policy_name - <<EOF
path "*" {
  capabilities = ["create", "read", "update", "delete", "list"]
}
EOF
fi

# Create roles
echo "$newlines"
role_name="vault-approle"
if vault read auth/approle/role/$role_name > /dev/null 2>&1; then
    echo "${BLUE}4. The AppRole '$role_name' already exists.${NC}"
else
    echo "${BLUE}4. Creating AppRole '$role_name'...${NC}"
    vault write auth/approle/role/$role_name policies="full-access"
fi

# Get Role ID and Secret ID, etc...
role_id=$(vault read auth/approle/role/vault-approle/role-id | grep 'role_id' | awk '{print $2}')

secret_id=$(vault write -f auth/approle/role/vault-approle/secret-id | grep 'secret_id ' | awk '{print $2}')

if [ -z "$role_id" ] || [ -z "$secret_id" ]; then
    echo "Error: Not found role_id or secret_id"
    exit 1
fi

echo "  Role ID: $role_id"
echo "  Secret ID: $secret_id"

# Login with approle
echo "$newlines"
echo "${BLUE}5. Login with approle...${NC}"
vault write auth/approle/login role_id=$role_id secret_id=$secret_id

# Write secrets, db config, rabbitmq config
echo "$newlines"
echo "${BLUE}6. Writing secrets...${NC}"
vault kv put -mount=vault-keyvalue ms-archetype \
    Security:ClientId=a74cb192-598c-4757-95ae-b315793bbbca \
    Security:ValidAudiences:0=a74cb192-598c-4757-95ae-b315793bbbca \
    Security:ValidAudiences:1=api://a74cb192-598c-4757-95ae-b315793bbbca
    
vault kv get -mount=vault-keyvalue ms-archetype

# Write database configuration
echo "$newlines"
echo "${BLUE}7. Writing database configuration...${NC}"
vault write vault-database/config/db-ms-archetype \
    plugin_name=mongodb-database-plugin \
    allowed_roles="ms-archetype-mongo-role" \
    connection_url="mongodb://{{username}}:{{password}}@mongo:27017/admin?ssl=false" \
    username="admin" \
    password="password"

vault write vault-database/roles/ms-archetype-mongo-role \
    db_name=db-ms-archetype \
    creation_statements='{ "db": "admin", "roles": [{ "role": "readWrite", "db": "db-ms-archetype" }] }' \
    default_ttl="1h" \
    max_ttl="24h"

vault read vault-database/creds/ms-archetype-mongo-role

# Write rabbitmq configuration
echo "$newlines"
echo "${BLUE}8. Writing rabbitmq configuration...${NC}"

sleep 12

vault write vault-rabbitmq/config/connection \
    connection_uri="http://rabbitmq:15672" \
    username="admin" \
    password="password"

vault write vault-rabbitmq/roles/ms-archetype-rabbitmq-role \
    vhosts='{"/":{"write": ".*", "read": ".*", "configure": ".*"}}'

vault read vault-rabbitmq/creds/ms-archetype-rabbitmq-role