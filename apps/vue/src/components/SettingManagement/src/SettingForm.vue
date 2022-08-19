<template>
  <Card :title="L('Settings')">
    <Form :label-col="{ span: 5 }" :wrapper-col="{ span: 15 }">
      <Tabs
        v-model:activeKey="activeTabKey"
        :tab-position="tabPosition"
        :style="tabsStyle.style"
        :tabBarStyle="tabsStyle.tabBarStyle"
      >
        <TabPane
          v-for="(group, tabIndex) in settingGroups"
          :key="tabIndex"
          :tab="group.displayName"
        >
          <Collapse :defaultActiveKey="expandedCollapseKeys(group)">
            <CollapsePanel
              v-for="setting in group.settings"
              :key="setting.displayName"
              :header="setting.displayName"
            >
              <template v-for="detail in setting.details" :key="detail.name">
                <slot
                  v-if="detail.slot"
                  :name="detail.slot"
                  :detail="detail"
                  :change="detail.valueType === 2 ? handleCheckChange : handleValueChange"
                />
                <FormItem v-else :label="detail.displayName" :extra="detail.description">
                  <!-- <Input type="text" v-model="detail.value" /> -->
                  <Password
                    v-if="detail.valueType === 0 && detail.isEncrypted"
                    v-model:value="detail.value"
                    :placeholder="detail.description"
                    @input="handleValueChange(detail)"
                  />
                  <BInput
                    v-if="detail.valueType === 0 && !detail.isEncrypted"
                    v-model:value="detail.value"
                    :placeholder="detail.description"
                    type="text"
                    @input="handleValueChange(detail)"
                  />
                  <BInput
                    v-if="detail.valueType === 1"
                    v-model:value="detail.value"
                    :placeholder="detail.description"
                    type="number"
                    @input="handleValueChange(detail)"
                  />
                  <DatePicker
                    v-if="detail.valueType === 3"
                    :value="detail.value ? dayjs(detail.value, 'YYYY-MM-DD') : ''"
                    :placeholder="detail.description"
                    style="width: 100%"
                    @change="handleDateChange($event, detail)"
                  />
                  <Select
                    v-if="detail.valueType === 5"
                    v-model:value="detail.value"
                    @change="handleValueChange(detail)"
                  >
                    <SelectOption
                      v-for="option in detail.options"
                      :key="option.value"
                      :disabled="option.value === detail.value"
                    >
                      {{ option.name }}
                    </SelectOption>
                  </Select>
                  <Checkbox
                    v-if="detail.valueType === 2"
                    :checked="detail.value === 'true'"
                    @change="handleCheckChange(detail)"
                  >
                    {{ detail.displayName }}
                  </Checkbox>
                </FormItem>
              </template>
            </CollapsePanel>
          </Collapse>
        </TabPane>
      </Tabs>
      <FormItem style="margin-top: 20px">
        <a-button
          v-if="updateSetting.settings.length > 0"
          type="primary"
          style="width: 150px"
          postIcon="ant-design:setting-outlined"
          :loading="saving"
          @click="handleSubmit"
        >
          {{ sumbitButtonTitle }}
        </a-button>
      </FormItem>
    </Form>
  </Card>
</template>

<script lang="ts">
  import dayjs from 'dayjs';
  import { computed, defineComponent, ref, toRaw } from 'vue';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useTabsStyle } from '/@/hooks/component/useStyles';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import {
    Card,
    Checkbox,
    Tabs,
    Collapse,
    Form,
    Input,
    Select,
    Row,
    Col,
    DatePicker,
  } from 'ant-design-vue';
  import { Input as BInput } from '/@/components/Input';
  import { formatToDate } from '/@/utils/dateUtil';
  import { SettingGroup, SettingsUpdate } from '/@/api/settings/model/settingModel';

  const props = {
    settingGroups: {
      type: Array as PropType<Array<SettingGroup>>,
      required: true,
    },
    saveApi: {
      type: Function as PropType<(...args: any) => Promise<any>>,
      required: true,
    },
    tabPosition: {
      type: String as PropType<'left' | 'right' | 'top' | 'bottom'>,
      default: 'top',
    },
  } as const; // 对于存在必输项的props see: https://blog.csdn.net/q535999731/article/details/109578885

  export default defineComponent({
    components: {
      Card,
      Checkbox,
      Collapse: Collapse,
      CollapsePanel: Collapse.Panel,
      DatePicker,
      Form: Form,
      FormItem: Form.Item,
      BInput,
      Select,
      SelectOption: Select.Option,
      Tabs: Tabs,
      TabPane: Tabs.TabPane,
      Password: Input.Password,
      Row,
      Col,
    },
    props,
    emits: ['change'],
    setup(props, { emit }) {
      const { L } = useLocalization('AbpSettingManagement');
      const activeTabKey = ref(0);
      const saving = ref(false);
      const updateSetting = ref(new SettingsUpdate());
      const { createMessage } = useMessage();
      const { success } = createMessage;

      const tabsStyle = useTabsStyle(
        props.tabPosition,
        {}, 
        {
          top: props.tabPosition === 'top' ? '80px' : '0'
        });
      const sumbitButtonTitle = computed(() => {
        if (saving.value) {
          return L('Save');
        }
        return L('Save');
      });
      const expandedCollapseKeys = computed(() => {
        return (group) => {
          const keys = group.settings.map((s) => {
            return s.displayName;
          });
          return keys;
        };
      });

      function handleCheckChange(setting) {
        if (setting.value === 'true') {
          setting.value = 'false';
        } else {
          setting.value = 'true';
        }
        handleValueChange(setting);
      }

      function handleDateChange(e, setting) {
        setting.value = dayjs.isDayjs(e) ? formatToDate(e) : '';
        handleValueChange(setting);
      }

      function handleValueChange(setting) {
        const index = updateSetting.value.settings.findIndex((s) => s.name === setting.name);
        if (index >= 0) {
          updateSetting.value.settings[index].value = setting.value;
        } else {
          updateSetting.value.settings.push({
            name: setting.name,
            value: setting.value,
          });
        }
      }

      function handleSubmit() {
        saving.value = true;
        props
          .saveApi(toRaw(updateSetting.value))
          .then(() => {
            success(L('SuccessfullySaved'));
            emit('change', toRaw(updateSetting.value));
          })
          .finally(() => {
            saving.value = false;
          });
      }

      return {
        L,
        dayjs,
        saving,
        tabsStyle,
        activeTabKey,
        updateSetting,
        sumbitButtonTitle,
        expandedCollapseKeys,
        handleCheckChange,
        handleDateChange,
        handleValueChange,
        handleSubmit,
      };
    },
  });
</script>
