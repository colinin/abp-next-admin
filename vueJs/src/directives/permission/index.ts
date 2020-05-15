import { DirectiveOptions } from 'vue'
import { PermissionModule } from '@/store/modules/permission'

export const permission: DirectiveOptions = {
  inserted(el, binding) {
    const { value } = binding
    const permissions = PermissionModule.authorizedPermissions
    if (value && value instanceof Array && value.length > 0) {
      const permissionRoles = value
      const hasPermission = permissions.some(permission => {
        return permissionRoles.includes(permission)
      })
      if (!hasPermission) {
        el.parentNode && el.parentNode.removeChild(el)
      }
    } else {
      throw new Error('need roles! Like v-permission="[\'admin\',\'editor\']"')
    }
  }
}
