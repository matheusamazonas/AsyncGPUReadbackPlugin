#!/bin/sh -ex
# $ANDROID_NDK_ROOT/ndk-build V=1 NDK_PROJECT_PATH=. NDK_APPLICATION_MK=Application.mk $*
$ANDROID_NDK_ROOT/ndk-build NDK_PROJECT_PATH=. NDK_APPLICATION_MK=Application.mk $*
# rm -rf ../../Android/*
# mv -f libs/* ../../Android/
