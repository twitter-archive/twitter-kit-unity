package com.twitter.sdk.android.unity;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;

import com.twitter.sdk.android.tweetcomposer.TweetUploadService;

public class TweetReceiver extends BroadcastReceiver {

    @Override
    public void onReceive(Context context, Intent intent) {
        if (intent != null && intent.getAction() != null) {
            if (TweetUploadService.UPLOAD_SUCCESS.equals(intent.getAction())) {
                Bundle intentExtras = intent.getExtras();
                if (intentExtras != null) {
                    sendSuccessMessage(intentExtras.getLong(TweetUploadService.EXTRA_TWEET_ID));
                }
            } else if (TweetUploadService.UPLOAD_FAILURE.equals(intent.getAction())) {
                sendFailureMessage();
            } else if (TweetUploadService.TWEET_COMPOSE_CANCEL.equals(intent.getAction())) {
                sendCancelMessage();
            }
        }
    }

    private void sendSuccessMessage(Long tweetId) {
        final UnityMessage message = new UnityMessage.Builder()
                .setMethod("TweetComplete")
                .setData(Long.toString(tweetId))
                .build();
        message.send();
    }

    private void sendFailureMessage() {
        final UnityMessage message = new UnityMessage.Builder()
                .setMethod("TweetFailed")
                .setData("")
                .build();
        message.send();
    }

    private void sendCancelMessage() {
        final UnityMessage message = new UnityMessage.Builder()
                .setMethod("TweetCancelled")
                .build();
        message.send();
    }
}
