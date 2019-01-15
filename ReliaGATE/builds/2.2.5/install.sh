#!/bin/sh

latadir='/lata'

if [ ! -e $latadir ]; then
  echo creating directories
  mkdir /lata
  mkdir /lata/rc
else
  cd $latadir
  echo cleaning old directories, if any
  rm -r gold
  echo copying old rc to gold, if any
  cp -r rc gold
  echo removing old rc, if any
  rm -r rc
  echo removing old tgz, if any
  rm /lata/*.tgz
fi

echo copying tarball
cp /usbflash/*.tgz /lata
cd $latadir

echo extracting tarball
tar xzvf ./*.tgz

echo move tarball to history directory
mkdir /lata/history
mv /lata/*.tgz /lata/history

echo installing files
cd /lata/rc
chmod u+x ./dir_install
./dir_install

echo Unmount and remove USB flash drives and then reboot
echo "umount /dev/sdc1"
