import { ref, computed } from 'vue'

export interface UserInfo {
  sub: string
  name: string
  email: string
  username: string
  roles: string[]
}

export function useAuth() {
  const user = ref<UserInfo | null>(null)
  const isAuthenticated = computed(() => user.value !== null)

  async function fetchUser() {
    try {
      const res = await fetch('/auth/me')
      if (res.ok) user.value = await res.json()
      else user.value = null
    } catch {
      user.value = null
    }
  }

  function login(returnUrl = '/') {
    window.location.href = `/auth/login?returnUrl=${encodeURIComponent(returnUrl)}`
  }

  async function logout() {
    await fetch('/auth/logout', { method: 'POST' })
    user.value = null
    window.location.href = '/'
  }

  return { user, isAuthenticated, fetchUser, login, logout }
}
