#include <jni.h>
#include <com_eurotech_reliagate_io_i2cbus_impl_I2cBusImpl.h>
#include <linux/i2c-dev.h>
#include <i2cbus.h>
#include <stdlib.h>

JNIEXPORT jint JNICALL Java_com_eurotech_reliagate_io_i2cbus_impl_I2cBusImpl_I2CBusOpen
  (JNIEnv *env, jobject class) {

	I2CBUS_HANDLE handle = OpenI2CBus();
	return (handle >= 0)? handle : -1;
}

JNIEXPORT jboolean JNICALL Java_com_eurotech_reliagate_io_i2cbus_impl_I2cBusImpl_I2CBusClose
  (JNIEnv *env, jobject class, jint handle) {

	return (CloseI2CBus(handle) == 0)? JNI_TRUE : JNI_FALSE;
}

JNIEXPORT jboolean JNICALL Java_com_eurotech_reliagate_io_i2cbus_impl_I2cBusImpl_I2CBusWriteByte
  (JNIEnv *env, jobject class, jint handle, jbyte slaveAddress, jbyte command, jbyte data) {

	I2C_REQUEST request;
	request.Address = slaveAddress;
	request.Command = command;
	request.Data[0] = data;

	jint status = I2CBusWriteByte(handle, &request);
	return (status == 0)? JNI_TRUE : JNI_FALSE;
}

JNIEXPORT jboolean JNICALL Java_com_eurotech_reliagate_io_i2cbus_impl_I2cBusImpl_I2CBusWriteWord
  (JNIEnv *env, jobject class, jint handle, jbyte slaveAddress, jbyte command, jshort data) {

	I2C_REQUEST request;
	request.Address = slaveAddress;
	request.Command = command;
	request.Data[0] = data & 0x0ff;
	request.Data[1] = (data >> 8) & 0x0ff;

	jint status = I2CBusWriteWord(handle, &request);
	return (status == 0)? JNI_TRUE : JNI_FALSE;
}

JNIEXPORT jboolean JNICALL Java_com_eurotech_reliagate_io_i2cbus_impl_I2cBusImpl_I2CBusWriteBlock
  (JNIEnv *env, jobject class, jint handle, jbyte slaveAddress, jbyte command, jbyteArray data) {

	I2C_REQUEST request;
	request.Address = slaveAddress;
	request.Command = command;
	request.BlockLength = sizeof(data)/sizeof(int);

	jboolean isCopy;
	jbyte *convertedData = (*env)->GetByteArrayElements(env, data, &isCopy);
	int i=0;
	for(i=0; i<request.BlockLength; i++) {
		request.Data[i] = convertedData[i];
	}
	(*env)->ReleaseByteArrayElements(env, data, convertedData, JNI_ABORT);

	jint status = I2CBusWriteBlock(handle, &request);
	return (status == 0)? JNI_TRUE : JNI_FALSE;
}


JNIEXPORT jbyte JNICALL Java_com_eurotech_reliagate_io_i2cbus_impl_I2cBusImpl_I2CBusReadByte
  (JNIEnv *env, jobject class, jint handle, jbyte slaveAddress, jbyte command) {

	jbyte ret = -1;
	I2C_REQUEST request;
	request.Address = slaveAddress;
	request.Command = command;

	int status = I2CBusReadByte(handle, &request);
	if (status == 0) {
		ret = request.Data[0];
	}
	return ret;
}

JNIEXPORT jshort JNICALL Java_com_eurotech_reliagate_io_i2cbus_impl_I2cBusImpl_I2CBusReadWord
  (JNIEnv *env, jobject class, jint handle, jbyte slaveAddress, jbyte command) {

	jshort ret = -1;
	I2C_REQUEST request;
	request.Address = slaveAddress;
	request.Command = command;

	int status = I2CBusReadWord(handle, &request);
	if (status == 0) {
		ret = ((request.Data[1] << 8) | request.Data[0]);
	}
	return ret;
}

JNIEXPORT jbyteArray JNICALL Java_com_eurotech_reliagate_io_i2cbus_impl_I2cBusImpl_I2CBusReadBlock
  (JNIEnv *env, jobject class, jint handle, jbyte slaveAddress, jbyte command) {

	jbyteArray ret = NULL;
	I2C_REQUEST request;
	request.Address = slaveAddress;
	request.Command = command;
	//todo - add a passed parameter to specify block length?
	//       (set to zero for now)
	request.BlockLength = 0;

	int status = I2CBusReadBlock(handle, &request);
	if (status == 0) {
		ret = (*env)->NewByteArray(env, I2C_MAX_DATA_SIZE);
		if (ret != NULL) {
				(*env)->SetByteArrayRegion(env, ret, 0, I2C_MAX_DATA_SIZE, (jbyte *)&request.Data);
		}
	}

	return ret;
}

JNIEXPORT jint JNICALL Java_com_eurotech_reliagate_io_i2cbus_impl_I2cBusImpl_I2CBusGetLastError
  (JNIEnv *env, jobject class, jint handle) {
	return I2CBusGetLastError(handle);
}

JNIEXPORT jint JNICALL Java_com_eurotech_reliagate_io_i2cbus_impl_I2cBusImpl_I2CBusClearLastError
  (JNIEnv *env, jobject class, jint handle) {
	I2CBusSetLastError(handle, 0);
	return 0;
}
