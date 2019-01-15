//---------------------------------------------------------------------------------
//
// Copyright @ Eurotech, Inc. 2010
//
// Filename:	i2cbus.h
// Date:	May 21, 2010
//	
// Description: 
//
//	This file contains the definitions for the Catalyst I2CBus driver api.
//
// History:
//
//---------------------------------------------------------------------------------


#ifndef __I2CBUS_H__
#define __I2CBUS_H__

typedef int I2CBUS_HANDLE;

#define I2C_MAX_DATA_SIZE		128


typedef struct _I2C_REQUEST
{
	unsigned char Address;
	unsigned char Command;
	unsigned char BlockLength;
	unsigned char Data[I2C_MAX_DATA_SIZE];
} I2C_REQUEST;


// Error codes
#define I2CBUS_ERROR_BASE	0x7600

#define ERR_I2CBUS_UNKNOWN_ERROR			(I2CBUS_ERROR_BASE + 0x00)
#define ERR_I2CBUS_DRIVER_OPEN				(I2CBUS_ERROR_BASE + 0x01)
#define ERR_I2CBUS_DRIVER_CLOSE				(I2CBUS_ERROR_BASE + 0x02)
#define ERR_I2CBUS_UNKNOWN_EXCEPTION		(I2CBUS_ERROR_BASE + 0x03)
#define ERR_I2CBUS_ACCESS_DENIED			(I2CBUS_ERROR_BASE + 0x04)
#define ERR_I2CBUS_COPYING_DRIVER			(I2CBUS_ERROR_BASE + 0x05)
#define ERR_I2CBUS_INVALID_HANDLE			(I2CBUS_ERROR_BASE + 0x06)
#define ERR_I2CBUS_TIMEOUT					(I2CBUS_ERROR_BASE + 0x07)
#define ERR_I2CBUS_UNSUPPORTED_FEATURE		(I2CBUS_ERROR_BASE + 0x08)
#define ERR_I2CBUS_INVALID_PARAMETER		(I2CBUS_ERROR_BASE + 0x09)


#ifdef __cplusplus
extern "C" {
#endif

//
// This function call initializes the I2CBus, opens the driver and 
// allocates the resources associated with the I2CBus. All I2CBus API calls are valid 
// after making this call except to re-open the I2CBus.
//
I2CBUS_HANDLE OpenI2CBus(void);

//
// This function call deinitializes the I2CBus, closes the driver and 
// frees the resources associated with the I2CBus. No I2CBus API calls are valid after
// making this call except to re-open the I2CBus.
//
int CloseI2CBus( I2CBUS_HANDLE handle );

//
// Call this function to send a byte on the I2CBus..
// This call is valid after a successful OPEN call has been made.
//
// Parameters: handle - Handle to open I2CBus API
//			  request - input I2C_REQUEST structure 
//
// Returns: 0=SUCCESS, non-zero = FAIL
//
int I2CBusSendByte( I2CBUS_HANDLE handle, I2C_REQUEST *request );

//
// Call this function to receive a byte from the I2CBus..
// This call is valid after a successful OPEN call has been made.
//
// Parameters: handle - Handle to open I2CBus API
//			  request - input I2C_REQUEST structure 
//
// Returns: 0=SUCCESS, non-zero = FAIL
//
int I2CBusReceiveByte( I2CBUS_HANDLE handle, I2C_REQUEST *request );

//
// Call this function to write a byte on the I2CBus. This function
// requires the proper values to be setup in the I2C_REQUEST structure.
// This call is valid after a successful OPEN call has been made.
//
// Parameters: handle - Handle to open I2CBus API
//			  request - input I2C_REQUEST structure 
//
// Returns: 0=SUCCESS, non-zero = FAIL
//
int I2CBusWriteByte( I2CBUS_HANDLE handle, I2C_REQUEST *request );

//
// Call this function to reads a byte on the I2CBus. This function
// requires the proper values to be setup in the I2C_REQUEST structure.
// This call is valid after a successful OPEN call has been made.
//
// Parameters: handle - Handle to open I2CBus API
//	          request - input I2C_REQUEST structure 
//
// Returns: 0=SUCCESS, non-zero = FAIL
//
int I2CBusReadByte( I2CBUS_HANDLE handle, I2C_REQUEST *request );

//
// Call this function to write a word on the I2CBus. This function
// requires the proper values to be setup in the I2C_REQUEST structure.
// This call is valid after a successful OPEN call has been made.
//
// Parameters: handle - Handle to open I2CBus API
// 	          request - input I2C_REQUEST structure 
//
// Returns: 0=SUCCESS, non-zero = FAIL
//
int I2CBusWriteWord( I2CBUS_HANDLE handle, I2C_REQUEST *request );

//
// Call this function to reads a word on the I2CBus. This function
// requires the proper values to be setup in the I2C_REQUEST structure.
// This call is valid after a successful OPEN call has been made.
//
// Parameters: handle - Handle to open I2CBus API
//	          request - input I2C_REQUEST structure 
//
// Returns: 0=SUCCESS, non-zero = FAIL
//
int I2CBusReadWord( I2CBUS_HANDLE handle, I2C_REQUEST *request );

//
// Call this function to write a block up to 32 bytes on the I2CBus. This function
// requires the proper values to be setup in the I2C_REQUEST structure.
// This call is valid after a successful OPEN call has been made.
//
// Parameters: handle - Handle to open I2CBus API
//	          request - input I2C_REQUEST structure 
//
// Returns: 0=SUCCESS, non-zero = FAIL
//
int I2CBusWriteBlock( I2CBUS_HANDLE handle, I2C_REQUEST *request );

//
// Call this function to read a block of up to 32 bytes from the I2CBus. This function
// requires the proper values to be setup in the I2C_REQUEST structure.
// This call is valid after a successful OPEN call has been made.
//
// Parameters: handle - Handle to open I2CBus API
//	          request - input I2C_REQUEST structure 
//
// Returns: 0=SUCCESS, non-zero = FAIL
//
int I2CBusReadBlock( I2CBUS_HANDLE handle, I2C_REQUEST *request );

//
// Call this function to get the I2CBus driver version.
// This call is valid after a successful OPEN call has been made.
//
// Parameters: handle - Handle to open I2CBus API
//			  request - input I2C_REQUEST structure 
//
// Returns: 0=SUCCESS, non-zero = FAIL
//
int I2CBusGetVersion( I2CBUS_HANDLE handle, unsigned short *version );


//
// Call this function if an error is returned to retrieve the error code
//
// Parameters: handle - Handle to open I2CBus API
//		      request - input I2C_REQUEST structure 
//
// Returns: Error code
//
int I2CBusGetLastError( I2CBUS_HANDLE handle );


//
// Call this function if an error is returned to clear the error code
//
// Parameters: handle - Handle to open I2CBus API
//		       error - error code to set
//
//
void I2CBusSetLastError(I2CBUS_HANDLE handle, int error );

#ifdef __cplusplus
}
#endif


#endif  // end of __I2CBUS_H__

