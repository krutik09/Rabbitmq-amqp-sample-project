# Rabbitmq-amqp-sample-project
This is a sample project of which contains demo app publishing message at exchange and subscribers receiving with Rabbitmq as broker.

# what this project fullfiles?
> All demo applications are working on "Rabbimq.Client" nuget package which works by default on "AMQP" protocol.
> The publishers will publish messages on topic exchanges.
> The messages published on exchange are buy default passed in durable queue, so we are receiving messages in queue which is declared durable so that it can survive failures and remains persistent.
> Docker compose file is necessary to run all applications in same network so that they can communicate with each other.
> Docker compose override file is used when we need to enable management plugin.
> definitions.json file is used to provide users, permissions, topic-permissions etc at the boot time.
> rabbitmq.conf file will be used to change default rabbitmq configs.
> Volumes for logs , data and configs are declared.
> Data volume consists of rabbitmq db "mnesia" which stores every information. Every-time we use same hostname for rabbitmq and volumes are not deleted the data in it will persist even if container and images are removed.
> To run rabbitmq with enables management pass ENV=dev , and to use it as disable pass ENV=prod.

