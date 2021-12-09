export function groupBy<T>(array: T[], id: string) {
  const groups: { [key: string]: T[] } = {};
  array.forEach(function (o) {
    const group = String(o[id]);
    groups[group] = groups[group] || [];
    groups[group].push(o);
  });
  return groups;
}
