readonly workdir=$(cd $(dirname $0); pwd)
echo "当前工作目录: $workdir"

echo "开始构建前端UI应用界面"

cd $workdir"/../apps/vue"

rm -rf $workdir"/dist/*"

npm install --registry=http://registry.npm.taobao.org
npm run build:prod


rm -rf $workdir"/../../aspnet-core/services/Publish/client"
mkdir -p $workdir"/../../aspnet-core/services/Publish/client/docker/nginx"


cp -r -f $workdir"/dist" $workdir"/../../aspnet-core/services/Publish/client/dist"
cp -r -f $workdir"/docker/nginx/nginx.conf" $workdir"/../../aspnet-core/services/Publish/client/docker/nginx/nginx.conf"
cp -r -f $workdir"/docker/nginx/default.conf" $workdir"/../../aspnet-core/services/Publish/client/docker/nginx/default.conf"
cp -r -f $workdir"/Dockerfile" $workdir"/../../aspnet-core/services/Publish/client/Dockerfile"
