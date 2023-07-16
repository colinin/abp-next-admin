abstract class CloneObject<T> {
  T clone();

  T cloneWith(Function(T) update) {
    var obj = clone();
    update(obj);
    return obj;
  }

  void deepWith(Function(T) update) {
    update(this as T);
  }
}