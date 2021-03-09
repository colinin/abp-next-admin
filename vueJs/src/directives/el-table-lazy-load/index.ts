import { DirectiveOptions } from 'vue'

export const elTableLazyLoad: DirectiveOptions = {
  inserted(el, binding) {
    const selectWrap = el.querySelector('.el-table__body-wrapper')
    if (selectWrap) {
      selectWrap.addEventListener('scroll', function() {
        console.log('onscroll directive')
        let sign = 0
        const scrollDistance = selectWrap.scrollHeight - selectWrap.scrollTop - selectWrap.clientHeight
        if (scrollDistance <= sign) {
          binding.value()
        }
      })
    }
  }
}
