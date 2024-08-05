<template>
  <List :class="prefixCls" bordered :pagination="getPagination">
    <template v-for="item in getData" :key="item.id">
      <ListItem class="list-item">
        <ListItemMeta>
          <template #title>
            <div class="title">
              <TypographyParagraph
                @click="handleTitleClick(item)"
                style="width: 100%; margin-bottom: 0 !important"
                :style="{ cursor: isTitleClickable ? 'pointer' : '' }"
                :delete="!!item.titleDelete"
                :ellipsis="
                  $props.titleRows && $props.titleRows > 0
                    ? { rows: $props.titleRows, tooltip: !!item.title }
                    : false
                "
                :content="item.title"
              />
              <div class="extra" v-if="item.extra">
                <Tag class="tag" :color="item.color">
                  {{ item.extra }}
                </Tag>
              </div>
            </div>
          </template>

          <template #avatar>
            <Avatar v-if="item.avatar" class="avatar" :src="item.avatar" />
            <span v-else> {{ item.avatar }}</span>
          </template>

          <template #description>
            <div>
              <div class="description" v-if="item.description">
                <TypographyParagraph
                  @click="handleContentClick(item)"
                  style="width: 100%; margin-bottom: 0 !important"
                  :style="{ cursor: isContentClickable ? 'pointer' : '' }"
                  :ellipsis="
                    $props.descRows && $props.descRows > 0
                      ? { rows: $props.descRows, tooltip: !!item.description }
                      : false
                  "
                  :content="getContent(item)"
                />
              </div>
              <div class="datetime">
                {{ item.datetime }}
              </div>
            </div>
          </template>
        </ListItemMeta>
      </ListItem>
    </template>
    <template #footer>
      <slot name="footer"></slot>
    </template>
  </List>
</template>
<script lang="ts" setup>
  import { computed, PropType, ref, watchEffect, unref } from 'vue';
  import { ListItem } from './data';
  import { useDesign } from '/@/hooks/web/useDesign';
  import { List, Avatar, Tag, Typography } from 'ant-design-vue';
  import { NotificationContentType } from '/@/api/messages/notifications/model';
  import { isNumber } from '/@/utils/is';

  const TypographyParagraph = Typography.Paragraph;
  const ListItem = List.Item;
  const ListItemMeta = List.Item.Meta;

  const emits = defineEmits(['update:currentPage']);
  const props = defineProps({
    list: {
      type: Array as PropType<ListItem[]>,
      default: () => [],
    },
    pageSize: {
      type: [Boolean, Number] as PropType<Boolean | Number>,
      default: 5,
    },
    currentPage: {
      type: Number,
      default: 1,
    },
    titleRows: {
      type: Number,
      default: 1,
    },
    descRows: {
      type: Number,
      default: 2,
    },
    onTitleClick: {
      type: Function as PropType<(Recordable) => void>,
    },
    onContentClick: {
      type: Function as PropType<(Recordable) => void>,
    }
  });

  const { prefixCls } = useDesign('header-notify-list');
  const current = ref(props.currentPage || 1);
  const getData = computed(() => {
    const { pageSize, list } = props;
    if (pageSize === false) return [];
    let size = isNumber(pageSize) ? pageSize : 10;
    return list.slice(size * (unref(current) - 1), size * unref(current));
  });
  const getContent = computed(() => {
    return (item: ListItem) => {
      switch (item.contentType) {
        default:
        case NotificationContentType.Text:
          return item.description;
        case NotificationContentType.Html:
        case NotificationContentType.Json:
        case NotificationContentType.Markdown:
          return item.title;
      }
    };
  });
  watchEffect(() => {
    current.value = props.currentPage;
  });
  const isTitleClickable = computed(() => !!props.onTitleClick);
  const isContentClickable = computed(() => !!props.onContentClick);
  const getPagination = computed(() => {
    const { list, pageSize } = props;
    if (pageSize === false) return false;
    const size = isNumber(pageSize) ? pageSize : 5;
    if (size > 0 && list && list.length > size) {
      return {
        total: list.length,
        pageSize: size,
        //size: 'small',
        current: unref(current),
        onChange(page) {
          current.value = page;
          emits('update:currentPage', page);
        },
      };
    } else {
      return false;
    }
  });

  function handleTitleClick(item: ListItem) {
    props.onTitleClick && props.onTitleClick(item);
  }

  function handleContentClick(item: ListItem) {
    props.onContentClick && props.onContentClick(item);
  }
</script>
<style lang="less" scoped>
  @prefix-cls: ~'@{namespace}-header-notify-list';

  .@{prefix-cls} {
    &::-webkit-scrollbar {
      display: none;
    }

    ::v-deep(.ant-pagination-disabled) {
      display: inline-block !important;
    }

    &-item {
      padding: 6px;
      overflow: hidden;
      cursor: pointer;
      transition: all 0.3s;

      .title {
        margin-bottom: 8px;
        font-weight: normal;

        .extra {
          float: right;
          margin-top: -1.5px;
          margin-right: 0;
          font-weight: normal;

          .tag {
            margin-right: 0;
          }
        }

        .avatar {
          margin-top: 4px;
        }

        .description {
          font-size: 12px;
          line-height: 18px;
        }

        .datetime {
          margin-top: 4px;
          font-size: 12px;
          line-height: 18px;
        }
      }
    }
  }
</style>
