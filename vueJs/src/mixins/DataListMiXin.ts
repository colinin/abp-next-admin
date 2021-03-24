import { Component, Mixins } from 'vue-property-decorator'
import { PagedResultDto, ListResultDto, PagedAndSortedResultRequestDto } from '@/api/types'

import LocalizationMiXin from './LocalizationMiXin'
/**
 * 数据列表mixin
 * 复写大部分数据列表事件
 */
@Component
export default class DataListMiXin extends Mixins(LocalizationMiXin) {
  /** 数据列表 */
  public dataList = new Array<any>()
  /** 数据总数 */
  public dataTotal = 0
  /** 当前页码 */
  public currentPage = 1
  /** 是否正在加载数据 */
  public dataLoading = false
  /** 查询过滤器
   *如果继承自分页查询接口的其他过滤类型,需要重写初始化类型
   */
  public dataFilter = new PagedAndSortedResultRequestDto()
  /** 页大小 */
  get pageSize() {
    return this.dataFilter.maxResultCount
  }

  set pageSize(value: number) {
    this.dataFilter.maxResultCount = value
  }

  protected processDataFilter() {
    this.dataFilter.skipCount = this.currentPage
    this.dataFilter.maxResultCount = this.pageSize
  }

  /**
   * 刷新数据
   */
  protected refreshData() {
    this.dataLoading = true
    this.getList(this.dataFilter)
      .then(res => {
        this.dataList = res.items
        this.dataTotal = res.items.length
        this.onDataLoadCompleted()
      })
      .finally(() => {
        this.dataLoading = false
      })
  }

  /**
   * 刷新分页数据
   */
  protected refreshPagedData() {
    this.dataLoading = true
    // 这里还可以处理对于过滤器的变动
    // 例如 abp 框架的skipCount区别于常见的pageNumber
    this.processDataFilter()
    this.getPagedList(this.dataFilter)
      .then(res => {
        this.dataList = res.items
        this.dataTotal = res.totalCount
        this.onDataLoadCompleted()
      })
      .finally(() => {
        this.dataLoading = false
      })
  }

  /**
   * 重写已执行具体查询数据逻辑
   */
  protected getList(filter: any): Promise<ListResultDto<any>> {
    console.log(filter)
    return this.getEmptyList()
  }

  /** 获取空数据 */
  protected getEmptyList() {
    return new Promise<ListResultDto<any>>((resolve) => {
      return resolve(new ListResultDto<any>())
    })
  }

  /** 重置列表数据 */
  protected resetList() {
    this.currentPage = 1
    this.refreshData()
  }

  /**
   * 重写以执行具体查询分页数据逻辑
   * @param filter 查询过滤器
   */
  protected getPagedList(filter: any): Promise<PagedResultDto<any>> {
    console.log(filter)
    return this.getEmptyPagedList()
  }

  /** 获取空分页数据 */
  protected getEmptyPagedList() {
    return new Promise<PagedResultDto<any>>((resolve) => {
      return resolve(new PagedResultDto<any>())
    })
  }

  /** 重置分页数据 */
  protected resetPagedList() {
    this.currentPage = 1
    this.refreshPagedData()
  }

  /** 数据加载完毕事件 */
  protected onDataLoadCompleted() {
    this.dataLoading = false
  }

  /**
   * 排序变更事件
   * @param column 事件列
   */
  protected handleSortChange(column: any) {
    this.dataFilter.sorting = column.prop
  }
}
