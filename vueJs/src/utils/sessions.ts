// Tenant
const tenantKey = 'vue_typescript_admin_tenant'
export const getTenant = () => sessionStorage.getItem(tenantKey)
export const setTenant = (tenantId: string) => sessionStorage.setItem(tenantKey, tenantId)
export const removeTenant = () => sessionStorage.removeItem(tenantKey)
