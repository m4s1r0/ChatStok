# ChatStok / Installation guide and use instruction 

## Installation 
Unzip all files

## Setup 
Go to Solution Explorer and open solution properties (right click on solution > Properties)
Set up the following startup projects:

  ChatStock.API | Start  
  ChatStock.Broker | Start  
  ChatStock.UI | Start

## Setup RabbitMQ (requires Docker) 
Enter following command on you terminal:

docker run -d --hostname my-rabbitmq-server --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management 

## Debug
Hit F5 to start debugging
