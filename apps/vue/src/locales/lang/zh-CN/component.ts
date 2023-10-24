export default {
  app: {
    searchNotData: '暂无搜索结果',
    toSearch: '确认',
    toNavigate: '切换',
  },
  countdown: {
    normalText: '获取验证码',
    sendText: '{0}秒后重新获取',
  },
  cropper: {
    selectImage: '选择图片',
    uploadSuccess: '上传成功',
    modalTitle: '头像上传',
    okText: '确认并上传',
    btn_reset: '重置',
    btn_rotate_left: '逆时针旋转',
    btn_rotate_right: '顺时针旋转',
    btn_scale_x: '水平翻转',
    btn_scale_y: '垂直翻转',
    btn_zoom_in: '放大',
    btn_zoom_out: '缩小',
    preview: '预览',
  },
  drawer: {
    loadingText: '加载中...',
    cancelText: '关闭',
    okText: '确认',
  },
  excel: {
    exportModalTitle: '导出数据',
    fileType: '文件类型',
    fileName: '文件名',
  },
  form: {
    putAway: '收起',
    unfold: '展开',

    maxTip: '字符数应小于{0}位',

    apiSelectNotFound: '请等待数据加载完成...',
  },
  icon: {
    placeholder: '点击选择图标',
    search: '搜索图标',
    copy: '复制图标成功!',
  },
  menu: {
    search: '菜单搜索',
  },
  modal: {
    cancelText: '关闭',
    okText: '确认',
    close: '关闭',
    maximize: '最大化',
    restore: '还原',
  },
  table: {
    settingDens: '密度',
    settingDensDefault: '默认',
    settingDensMiddle: '中等',
    settingDensSmall: '紧凑',
    settingColumn: '列设置',
    settingColumnShow: '列展示',
    settingIndexColumnShow: '序号列',
    settingSelectColumnShow: '勾选列',
    settingDragColumnShow: '拖拽列',
    settingFixedLeft: '固定到左侧',
    settingFixedRight: '固定到右侧',
    settingFullScreen: '全屏',
    index: '序号',
    total: '共 {total} 条数据',
    selectedRows: '已选择 {count} 项',
    deSelected: '取消选择',
    advancedSearch: {
      title: '高级查询',
      conditions: '查询条件',
      addCondition: '增加条件',
      delCondition: '删除条件',
      clearCondition: '清空条件',
      field: '字段',
      logic: '连接条件',
      and: '且',
      or: '或',
      comparison: '运算符',
      value: '比较值',
      equal: '等于',
      notEqual: '不等于',
      lessThan: '小于',
      lessThanOrEqual: '小于等于',
      greaterThan: '大于',
      greaterThanOrEqual: '大于等于',
      startsWith: '左包含',
      notStartsWith: '左不包含',
      endsWith: '右包含',
      notEndsWith: '右不包含',
      contains: '包含',
      notContains: '不包含',
      null: '空',
      notNull: '非空',
    }
  },
  time: {
    before: '前',
    after: '后',
    just: '刚刚',
    seconds: '秒',
    minutes: '分钟',
    hours: '小时',
    days: '天',
  },
  tree: {
    selectAll: '选择全部',
    unSelectAll: '取消选择',
    expandAll: '展开全部',
    unExpandAll: '折叠全部',
    checkStrictly: '层级关联',
    checkUnStrictly: '层级独立',
  },
  upload: {
    save: '保存',
    upload: '上传',
    imgUpload: '图片上传',
    uploaded: '已上传',

    operating: '操作',
    del: '删除',
    download: '下载',
    saveWarn: '请等待文件上传后，保存!',
    saveError: '没有上传成功的文件，无法保存!',

    preview: '预览',
    choose: '选择文件',

    accept: '支持{0}格式',
    acceptUpload: '只能上传{0}格式文件',
    maxSize: '单个文件不超过{0}MB',
    maxSizeMultiple: '只能上传不超过{0}MB的文件!',
    maxNumber: '最多只能上传{0}个文件',

    legend: '略缩图',
    fileName: '文件名',
    fileSize: '文件大小',
    fileStatue: '状态',

    startUpload: '开始上传',
    uploadSuccess: '上传成功',
    uploadError: '上传失败',
    uploading: '上传中',
    uploadWait: '请等待文件上传结束后操作',
    reUploadFailed: '重新上传失败文件',
  },
  verify: {
    error: '验证失败！',
    time: '验证校验成功,耗时{time}秒！',

    redoTip: '点击图片可刷新',

    dragText: '请按住滑块拖动',
    successText: '验证通过',
  },
  localizable_input: {
    placeholder: '请选择本地化资源',
    resources: {
      fiexed: {
        group: '自定义',
        label: '固定内容',
        placeholder: '请输入自定义内容',
      },
      localization: {
        group: '本地化',
        placeholder: '请选择名称',
      }
    }
  },
  extra_property_dictionary: {
    title: '扩展属性',
    key: '名称',
    value: '键值',
    actions: {
      title: '操作',
      create: '新增',
      update: '编辑',
      delete: '删除',
      clean: '清除'
    },
    validator: {
      duplicateKey: '已经添加了一个相同名称的键',
    },
  },
  value_type_nput: {
    type: {
      name: '类型',
      FREE_TEXT: {
        name: '自由文本',
      },
      TOGGLE: {
        name: '切换',
      },
      SELECTION: {
        name: '选择',
        displayText: '显示名称',
        displayTextNotBeEmpty: '显示名称不可为空',
        value: '选择项',
        duplicateKeyOrValue: '选择项的名称或值不允许重复',
        itemsNotBeEmpty: '可选择项列表不能为空',
        itemsNotFound: '选择项不包含在可选列表中',
        actions: {
          title: '操作',
          create: '新增',
          update: '编辑',
          delete: '删除',
          clean: '清除',
        },
        modal: {
          title: '选择项',
        },
      },
    },
    validator: {
      name: '验证器',
      isInvalidValue: '值未能通过 {0} 校验, 请检查验证器选项.',
      NULL: {
        name: '未定义',
      },
      BOOLEAN: {
        name: '布尔类型',
      },
      NUMERIC: {
        name: '数值类型',
        minValue: '最小值',
        maxValue: '最大值',
      },
      STRING: {
        name: '字符类型',
        allowNull: '允许空值',
        minLength: '最小长度',
        maxLength: '最大长度',
        regularExpression: '正则表达式',
      },
    },
  },
  simple_state_checking: {
    title: '状态检查',
    actions: {
      create: '新增',
      update: '编辑',
      delete: '删除',
      clean: '清除'
    },
    table: {
      name: '名称',
      properties: '属性',
      actions: '操作',
    },
    form: {
      name: '状态检查器',
    },
    requireAuthenticated: {
      title: '需要用户认证',
    },
    requireFeatures: {
      title: '检查所需功能',
      requiresAll: '要求所有',
      requiresAllDesc: '如果勾选,则需要启用所有选择的功能.',
      featureNames: '需要的功能',
    },
    requireGlobalFeatures: {
      title: '检查全局功能',
      featureNames: '需要的全局功能',
    },
    requirePermissions: {
      title: '检查所需权限',
      requiresAll: '要求所有',
      requiresAllDesc: '如果勾选,则需要拥有所有选择的权限.',
      permissions: '需要的权限',
    },
  }
};
