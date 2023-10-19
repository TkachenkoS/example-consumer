# Set Pactflow credentials
$pactBrokerBaseUri = "https://miviewis.pactflow.io/"
$pactBrokerToken = "ZRP32zTVVIR083gV58edSA"

# Path to the Pact files
$pactFilesPath = "/pacts/pactflow-example-consumer-pactflow-example-provider.json"

# Publish Pacts to Pactflow
docker run -v ${PWD}/pacts/pactflow-example-consumer-pactflow-example-provider.json:/pacts/pactflow-example-consumer-pactflow-example-provider.json --rm pactfoundation/pact-cli:latest pact-broker publish $pactFilesPath --broker-base-url $pactBrokerBaseUri --broker-token $pactBrokerToken --consumer-app-version 0.0.2