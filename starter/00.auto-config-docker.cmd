docker network create --subnet=172.18.0.0/16 nt

docker pull mysql
docker volume rm mysql-data
docker volume rm mysql-log
docker volume create mysql-data
docker volume create mysql-log
docker run --ip 172.18.0.10 -d --name mysql --net nt -v mysql-log:/var/log/mysql -v mysql-data:/var/lib/mysql -p 3306:3306 -p 33060:33060 -e MYSQL_ROOT_PASSWORD=123456 -d mysql --init-connect="SET collation_connection=utf8mb4_0900_ai_ci" --init-connect="SET NAMES utf8mb4" --skip-character-set-client-handshake

docker pull rabbitmq:management
docker volume rm rabbitmq-home
docker volume create rabbitmq-home
docker run --ip 172.18.0.40 -d -id --name=rabbitmq --net nt -v rabbitmq-home:/var/lib/rabbitmq -p 15672:15672 -p 5672:5672 -e RABBITMQ_DEFAULT_USER=admin -e RABBITMQ_DEFAULT_PASS=admin rabbitmq:management

docker pull redis
docker volume rm redis-home
docker volume create redis-home
docker run --ip 172.18.0.50 -d --net nt -p 6379:6379 --name redis -v redis-home:/data redis

docker pull docker.elastic.co/elasticsearch/elasticsearch:8.9.0
docker volume rm elasticsearch-home
docker volume create elasticsearch-home
docker run --ip 172.18.0.60 -d --name es --net nt -v elasticsearch-home:/usr/share/elasticsearch/data -e "discovery.type=single-node" -e ES_JAVA_OPTS="-Xms1G -Xmx1G"  -e xpack.security.enabled=false  -p 9200:9200 -p 9300:9300 -it docker.elastic.co/elasticsearch/elasticsearch:8.9.0

docker pull docker.elastic.co/kibana/kibana:8.9.0
docker run --ip 172.18.0.70 -d --name kib --net nt -p 5601:5601 docker.elastic.co/kibana/kibana:8.9.0
