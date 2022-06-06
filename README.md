# Files

## Clone auth repository
    git clone https://github.com/travel-bridge/auth.git

## Go to auth repository and run
    Make build

## Up development environment
    docker-compose -f docker-compose.development.yml up -d

## Down development environment
    docker-compose -f docker-compose.development.yml down

## Generate test token
    POST http://localhost:8010/connect/token
    CONTENT-TYPE application/x-www-form-urlencoded
    grant_type=password
    client_id=36c04531-8d7c-49bf-a94a-260689c16e00
    username=test
    password=test