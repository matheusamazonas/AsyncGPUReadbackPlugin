include $(CLEAR_VARS)

# override strip command to strip all symbols from output library; no need to ship with those..
# cmd-strip = $(TOOLCHAIN_PREFIX)strip $1 

# OPENCV_ROOT             := ./OpenCV-android-sdk
# # OPENCV_CAMERA_MODULES := on
# OPENCV_INSTALL_MODULES  := on
# OPENCV_LIB_TYPE         := SHARED
# # include ${OPENCV_ROOT}/sdk/native/jni/OpenCV.mk

# GLEW
# GLEW_ROOT := ../glew
# LOCAL_C_INCLUDES := ../glew
# include ${GLEW_ROOT}/include/GL/glew.h

LOCAL_C_INCLUDES := ../glew/src/.
# LOCAL_LDLIBS     := -lz -llog -landroid  -lEGL -lGLESv1_CM -lGLESv2 -lGLESv3
LOCAL_LDLIBS     := -lEGL -lGLESv1_CM -lGLESv2 -lGLESv3 -llog

LOCAL_ARM_MODE      := arm
LOCAL_PATH          := $(NDK_PROJECT_PATH)
LOCAL_MODULE        := libAsyncGPUReadbackPlugin
# LOCAL_CFLAGS        := -Werror
LOCAL_CFLAGS        := -Werror -std=c++11
LOCAL_CPP_EXTENSION := .cxx .cpp .cc
LOCAL_CPP_FEATURES  := rtti exceptions
LOCAL_SRC_FILES     := ../src/AsyncGPUReadbackPlugin.cpp

include $(BUILD_SHARED_LIBRARY)
