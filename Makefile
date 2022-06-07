.PHONY: all

REGISTRY?=local
BUILD_NUMBER?=latest
REVISION?=revision-not-specified
TEST_NETWORK?=host
TEST_ENVIRONMENT?=docker

build:
	@echo build services:
	docker build . --rm  --tag ${REGISTRY}/files_services:$(BUILD_NUMBER) --target services --build-arg revision='${REVISION}'

	@echo build worker:
	docker build . --rm  --tag ${REGISTRY}/files_worker:$(BUILD_NUMBER) --target worker --build-arg revision='${REVISION}'

	@echo build tests:
		docker build . --rm  --tag ${REGISTRY}/files_tests:$(BUILD_NUMBER) --target tests

test:
	@echo run tests:
	docker run --rm --network='${TEST_NETWORK}' -e ASPNETCORE_ENVIRONMENT=${TEST_ENVIRONMENT} ${REGISTRY}/files_tests:$(BUILD_NUMBER)