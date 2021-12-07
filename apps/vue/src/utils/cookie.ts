export function get(name) {
  let arr;
  const reg = new RegExp('(^| )' + name + '=([^;]*)(;|$)');
  if ((arr = document.cookie.match(reg))) return arr[2];
  else return null;
}

export function set(c_name, value, expiredays) {
  const exdate = new Date();
  exdate.setDate(exdate.getDate() + expiredays);
  document.cookie =
    c_name + '=' + escape(value) + (expiredays == null ? '' : ';expires=' + exdate.toUTCString());
}

export function del(name) {
  const exp = new Date();
  exp.setTime(exp.getTime() - 1);
  const cval = get(name);
  if (cval != null) document.cookie = name + '=' + cval + ';expires=' + exp.toUTCString();
}
