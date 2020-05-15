import { PermissionModule } from '@/store/modules/permission'

export function checkPermission(value: string[]): boolean {
  if (value && value instanceof Array && value.length > 0) {
    const permissions = PermissionModule.authorizedPermissions
    const permissionRoles = value
    const hasPermission = permissions.some(permission => {
      return permissionRoles.includes(permission)
    })
    return hasPermission
  } else {
    console.error('need roles! Like v-permission="[\'admin\',\'editor\']"')
    return false
  }
}
