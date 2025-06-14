defaultList)
                      list = list.slice();
                    list.unshift(algo);
                  }
                }
              }
            }
          }
          break;
        case 'remove':
          if (!Array.isArray(val))
            val = [val];
          if (Array.isArray(val)) {
            for (let j = 0; j < val.length; ++j) {
              const search = val[j];
              if (typeof search === 'string') {
                if (!search)
                  continue;
                const idx = list.indexOf(search);
                if (idx === -1)
                  continue;
                if (list === defaultList)
                  list = list.slice();
                list.splice(idx, 1);
              } else if (isRegExp(search)) {
                for (let k = 0; k < list.length; ++k) {
                  if (search.test(list[k])) {
                    if (list === defaultList)
                      list = list.slice();
                    list.splice(k, 1);
                    --k;
                  }
                }
              }
            }
          }
          break;
      }
    }

    return list;
  }

  return defaultList;
}

module.exports = {
  ChannelManager,
  generateAlgorithmList,
  onChannelOpenFailure,
  onCHANNEL_CLOSE,
  isWritable: (stream) => {
    // XXX: hack to workaround regression in node
    // See: https://github.com/nodejs/node/issues/36029
    return (stre