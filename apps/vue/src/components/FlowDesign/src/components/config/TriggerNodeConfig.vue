<template>
  <div>
    <Form layout="vertical">
      <FormItem label="选择触发的动作" name="text" class="user-type">
        <RadioGroup v-model:value="config.type">
          <Radio value="WEBHOOK">发送网络请求</Radio>
          <Radio value="EMAIL">发送邮件</Radio>
        </RadioGroup>
      </FormItem>
      <div v-if="config.type === 'WEBHOOK'">
        <FormItem label="请求地址" name="text">
          <InputGroup compact>
            <Select style="width: 20%" v-model:value="config.http.method" placeholder="URL">
              <Option value="GET">GET</Option>
              <Option value="POST">POST</Option>
              <Option value="PUT">PUT</Option>
              <Option value="DELETE">DELETE</Option>
            </Select>
            <Input
              style="width: 80%"
              placeholder="请输入URL地址"
              size="middle"
              v-model:value="config.http.url"
            />
          </InputGroup>
        </FormItem>
        <FormItem name="text">
          <template #label>
            <span style="margin-right: 10px">Header请求头</span>
            <Button type="link" @click="addItem(config.http.headers)"> + 添加</Button>
          </template>
          <div
            style="margin-top: 6px"
            v-for="(header, index) in config.http.headers"
            :key="header.name"
          >
            -
            <Input
              placeholder="参数名"
              size="small"
              style="width: 100px"
              v-model:value="header.name"
            />
            <RadioGroup size="small" style="margin: 0 5px" v-model:value="header.isField">
              <RadioButton :value="true">表单</RadioButton>
              <RadioButton :value="false">固定</RadioButton>
            </RadioGroup>
            <Select
              v-if="header.isField"
              style="width: 180px"
              v-model:value="header.value"
              size="small"
              placeholder="请选择表单字段"
            >
              <Option v-for="form in forms" :key="form.id" :value="form.title">{{
                form.title
              }}</Option>
            </Select>
            <Input
              v-else
              placeholder="请设置字段值"
              size="small"
              v-model:value="header.value"
              style="width: 180px"
            />
            <DeleteOutlined
              @click="delItem(config.http.headers, index)"
              style="margin-left: 5px; color: #c75450; cursor: pointer"
            />
          </div>
        </FormItem>
        <FormItem name="text">
          <template #label>
            <span style="margin-right: 10px">Header请求参数 </span>
            <Button style="margin-right: 20px" type="link" @click="addItem(config.http.params)">
              + 添加</Button
            >
            <span>参数类型 - </span>
            <RadioGroup size="small" style="margin: 0 5px" v-model:value="config.http.contentType">
              <RadioButton value="JSON">json</RadioButton>
              <RadioButton value="FORM">form</RadioButton>
            </RadioGroup>
          </template>
          <div
            style="margin-top: 6px"
            v-for="(param, index) in config.http.params"
            :key="param.name"
          >
            -
            <Input
              placeholder="参数名"
              size="small"
              style="width: 100px"
              v-model:value="param.name"
            />
            <RadioGroup size="small" style="margin: 0 5px" v-model:value="param.isField">
              <RadioButton :value="true">表单</RadioButton>
              <RadioButton :value="false">固定</RadioButton>
            </RadioGroup>
            <Select
              v-if="param.isField"
              style="width: 180px"
              v-model:value="param.value"
              size="small"
              placeholder="请选择表单字段"
            >
              <Option v-for="form in forms" :key="form.id" :value="form.title">{{
                form.title
              }}</Option>
            </Select>
            <Input
              v-else
              placeholder="请设置字段值"
              size="small"
              v-model:value="param.value"
              style="width: 180px"
            />
            <DeleteOutlined
              @click="delItem(config.http.params, index)"
              style="margin-left: 5px; color: #c75450; cursor: pointer"
            />
          </div>
          <div> </div>
        </FormItem>
        <FormItem name="text">
          <template #label>
            <span>请求结果处理</span>
            <span style="margin-left: 20px">自定义脚本: </span>
            <Switch v-model:checked="config.http.handlerByScript"></Switch>
          </template>
          <span class="item-desc" v-if="config.http.handlerByScript">
            👉 返回值为 ture 则流程通过，为 false 则流程将被驳回
            <div
              >支持函数
              <span style="color: dodgerblue"
                >setFormByName(
                <span style="color: #939494">'表单字段名', '表单字段值'</span>
                )</span
              >
              可改表单数据
            </div>
          </span>
          <span class="item-desc" v-else>👉 无论请求结果如何，均通过</span>
          <div v-if="config.http.handlerByScript">
            <div>
              <span>请求成功😀：</span>
              <Textarea v-model:value="config.http.success" :rows="3"></Textarea>
            </div>
            <div>
              <span>请求失败😥：</span>
              <Textarea v-model:value="config.http.fail" :rows="3"></Textarea>
            </div>
          </div>
        </FormItem>
      </div>
      <div v-else-if="config.type === 'EMAIL'">
        <FormItem label="邮件主题" name="text">
          <Input placeholder="请输入邮件主题" size="middle" v-model:value="config.email.subject" />
        </FormItem>
        <FormItem label="收件方" name="text">
          <Select
            size="small"
            style="width: 100%"
            v-model:value="config.email.to"
            filterable
            multiple
            allow-create
            default-first-option
            placeholder="请输入收件人"
          >
            <Option v-for="item in config.email.to" :key="item" :value="item">{{ item }}</Option>
          </Select>
        </FormItem>
        <FormItem label="邮件正文" name="text">
          <Textarea
            v-model:value="config.email.content"
            :rows="4"
            placeholder="邮件内容，支持变量提取表单数据 ${表单字段名} "
          ></Textarea>
        </FormItem>
      </div>
    </Form>
  </div>
</template>

<script setup lang="ts">
  import { computed, reactive, toRefs } from 'vue';
  import { Button, Form, Input, Select, Switch, Textarea, Radio } from 'ant-design-vue';
  import { DeleteOutlined } from '@ant-design/icons-vue';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useFlowStoreWithOut } from '/@/store/modules/flow';

  const FormItem = Form.Item;
  const Option = Select.Option;
  const InputGroup = Input.Group;
  const RadioGroup = Radio.Group;
  const RadioButton = Radio.Button;

  const flowStore = useFlowStoreWithOut();

  defineProps({
    config: {
      type: Object,
      default: () => {
        return {};
      },
    },
  });
  const forms = computed(() => {
    return flowStore.design.formItems || [];
  });
  const state = reactive({
    cmOptions: {
      tabSize: 4, // tab
      indentUnit: 4,
      styleActiveLine: true, // 高亮选中行
      lineNumbers: true, // 显示行号
      styleSelectedText: true,
      line: true,
      foldGutter: true, // 块槽
      gutters: ['CodeMirror-linenumbers', 'lock', 'warn'],
      highlightSelectionMatches: { showToken: /w/, annotateScrollbar: true }, // 可以启用该选项来突出显示当前选中的内容的所有实例
      mode: 'javascript',
      // hint.js options
      hintOptions: {
        // 当匹配只有一项的时候是否自动补全
        completeSingle: false,
      },
      // 快捷键 可提供三种模式 sublime、emacs、vim
      keyMap: 'sublime',
      matchBrackets: true,
      showCursorWhenSelecting: false,
      // scrollbarStyle:null,
      // readOnly:true,  //是否只读
      theme: 'material', // 主题 material
      extraKeys: { Ctrl: 'autocomplete' }, // 可以用于为编辑器指定额外的键绑定，以及keyMap定义的键绑定
      lastLineBefore: 0,
    },
  });
  const { createMessage } = useMessage();

  function addItem(items: any) {
    if (
      items.length > 0 &&
      (items[items.length - 1].name.trim() === '' || items[items.length - 1].value.trim() === '')
    ) {
      createMessage.warning('请完善之前项后在添加');
      return;
    }
    items.push({ name: '', value: '', isField: true });
  }

  function delItem(items: any, index: number) {
    items.splice(index, 1);
  }

  defineExpose({
    ...toRefs(state),
  });
</script>

<style lang="less" scoped>
  .item-desc {
    color: #939494;
  }
</style>
