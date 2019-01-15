CC=/home/wruser/WindRiver/workspace/WRLinux-Platform_prj/host-cross/i586-wrs-linux-gnu/x86-linux2/atom-target-linux-gnu-gcc
CFLAGS = -g -Wall -O2 -fPIC

INCLUDES = -I/usr/lib/jvm/java-1.6.0-openjdk-1.6.0.0/include/ -I/usr/lib/jvm/java-1.6.0-openjdk-1.6.0.0/include/linux -I/home/wruser/WindRiver/workspace/WRLinux-Platform_prj/export/sysroot/kbc_km2m806-glibc_std/sysroot/usr/include -I.

LINKER=/home/wruser/WindRiver/workspace/WRLinux-Platform_prj/host-cross/i586-wrs-linux-gnu/x86-linux2/atom-target-linux-gnu-ld
LNKFLAGS = -shared -o

I2CBUS_LIB_NAME=libi2cbus.so
I2CBUS_JNI_OBJS=i2cbus.o

all : release

release : i2cbus_jni_so
	cp ${I2CBUS_LIB_NAME} ../../../lib/linux/wrl/wrl_4.3_glibc_std/

i2cbus_jni_so: ${I2CBUS_JNI_OBJS}
	$(LINKER) -v $(LNKFLAGS) ${I2CBUS_LIB_NAME} ${I2CBUS_JNI_OBJS} -L../lib -lCatalystI2CBus
	
i2cbus.o:
	${CC} ${CFLAGS} ${INCLUDES} i2cbus.c -c -o i2cbus.o
		
clean :
	rm -fvr *.o *.so
	rm -fvr ../../../lib/linux/wrl/wrl_4.3_glibc_std/*.so
