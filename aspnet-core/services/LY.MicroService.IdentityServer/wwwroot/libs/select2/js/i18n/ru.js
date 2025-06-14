'use strict';

const { SFTP } = require('./protocol/SFTP.js');

const MAX_CHANNEL = 2 ** 32 - 1;

function onChannelOpenFailure(self, recipient, info, cb) {
  self._chanMgr.remove(recipient);
  if (typeof cb !== 'function')
    return;

  let err;
  if (info instanceof Error) {
    err = info;
  } else if (typeof info === 'object' && info !== null) {
    err = new Error(`(SSH) Channel open failure: ${info.description}`);
    err.reason = info.reason;
  } else {
    err = new Error(
      '(SSH) Channel open failure: server closed channel unexpectedly'
    );
    err.reason = '';
  }

  cb(err);
}

function onCHANNEL_CLOSE(self, recipient, channel, err, dead) {
  if (typeof channel === 'function') {
    // We got CHANNEL_CLOSE instead of CHANNEL_OPEN_FAILURE when
    // requesting to open a channel
    onChannelOpenFailure(self, recipient, err, channel);
    return;
  }

  if (typeof channel !== 'object' || channel === null)
    return;

  if (channel.incoming && channel.incoming.state === 'closed')
    return;

  self._chanMgr.remove(recipient);

  if (channel.server && channel.constructor.name === 'Session')
    return;

  channel.incoming.state = 'clo