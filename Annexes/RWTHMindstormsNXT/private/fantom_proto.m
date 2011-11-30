function [methodinfo,structs,enuminfo,ThunkLibName]=fantom_proto
% Prototype file for library "fantom" (used in Windows USB communication)
%
% Syntax
%   [methodinfo,structs,enuminfo]=fantom_proto
%
% Description
%   This function was generated by loadlibrary.m from the header-files of
%   the official LEGO Mindstorms NXT Fantom SDK. Based on the modified
%   header-file "matlabwrapper.h" by Vital van Reeven:
%           http://forums.nxtasy.org/index.php?showtopic=2018
%           http://www.vitalvanreeven.nl/page156/fantomNXT.zip
%
% Signature
%   Author: Vital van Reeven, Linus Atorf (see AUTHORS)
%   Date: 2008/03/29
%   Copyright: 2007-2010, RWTH Aachen University
%
% ***********************************************************************************************
% *  This file is part of the RWTH - Mindstorms NXT Toolbox.                                    *
% *                                                                                             *
% *  The RWTH - Mindstorms NXT Toolbox is free software: you can redistribute it and/or modify  *
% *  it under the terms of the GNU General Public License as published by the Free Software     *
% *  Foundation, either version 3 of the License, or (at your option) any later version.        *
% *                                                                                             *
% *  The RWTH - Mindstorms NXT Toolbox is distributed in the hope that it will be useful,       *
% *  but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS  *
% *  FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.             *
% *                                                                                             *
% *  You should have received a copy of the GNU General Public License along with the           *
% *  RWTH - Mindstorms NXT Toolbox. If not, see <http://www.gnu.org/licenses/>.                 *
% ***********************************************************************************************

% To prevent warning 'MATLAB:loadlibrary:OldStyleMfile', this file was
% edited manually (the empty thunk-file output arg was added, just what the
% new loadlibrary generator does). We don't have a thunk-file, and we
% probably will not support 64bit platforms anyway as of now...

%This function was generated by loadlibrary.m parser version 1.1.6.22 on Tue Mar 11 16:26:30 2008
%perl options:'fantom.i -outfile=fantom_proto.m'
ival={cell(1,0)}; % change 0 to the actual number of functions to preallocate the data.
fcns=struct('name',ival,'calltype',ival,'LHS',ival,'RHS',ival,'alias',ival);
structs=[];enuminfo=[];fcnNum=1;
ThunkLibName=[];
% nFANTOM100_iNXTIterator nFANTOM100_createNXTIterator ( ViBoolean searchBluetooth , ViUInt32 bluetoothSearchTimeoutInSeconds , ViStatus * status ); 
fcns.name{fcnNum}='nFANTOM100_createNXTIterator'; fcns.calltype{fcnNum}='cdecl'; fcns.LHS{fcnNum}='uint32'; fcns.RHS{fcnNum}={'uint16', 'uint32', 'int32Ptr'};fcnNum=fcnNum+1;
% void nFANTOM100_destroyNXTIterator ( nFANTOM100_iNXTIterator iterPtr , ViStatus * status ); 
fcns.name{fcnNum}='nFANTOM100_destroyNXTIterator'; fcns.calltype{fcnNum}='cdecl'; fcns.LHS{fcnNum}=[]; fcns.RHS{fcnNum}={'uint32', 'int32Ptr'};fcnNum=fcnNum+1;
% void nFANTOM100_pairBluetooth ( ViConstString resourceName , ViConstString passkey , ViChar pairedResourceName [], ViStatus * status ); 
fcns.name{fcnNum}='nFANTOM100_pairBluetooth'; fcns.calltype{fcnNum}='cdecl'; fcns.LHS{fcnNum}=[]; fcns.RHS{fcnNum}={'int8Ptr', 'int8Ptr', 'int8Ptr', 'int32Ptr'};fcnNum=fcnNum+1;
% void nFANTOM100_unpairBluetooth ( ViConstString resourceName , ViStatus * status ); 
fcns.name{fcnNum}='nFANTOM100_unpairBluetooth'; fcns.calltype{fcnNum}='cdecl'; fcns.LHS{fcnNum}=[]; fcns.RHS{fcnNum}={'int8Ptr', 'int32Ptr'};fcnNum=fcnNum+1;
% ViBoolean nFANTOM100_isPaired ( ViConstString resourceName , ViStatus * status ); 
fcns.name{fcnNum}='nFANTOM100_isPaired'; fcns.calltype{fcnNum}='cdecl'; fcns.LHS{fcnNum}='uint16'; fcns.RHS{fcnNum}={'int8Ptr', 'int32Ptr'};fcnNum=fcnNum+1;
% nFANTOM100_iNXT nFANTOM100_createNXT ( ViConstString resourceString , ViStatus * status , ViBoolean checkFirmwareVersion ); 
fcns.name{fcnNum}='nFANTOM100_createNXT'; fcns.calltype{fcnNum}='cdecl'; fcns.LHS{fcnNum}='uint32'; fcns.RHS{fcnNum}={'int8Ptr', 'int32Ptr', 'uint16'};fcnNum=fcnNum+1;
% void nFANTOM100_destroyNXT ( nFANTOM100_iNXT nxtPtr , ViStatus * status ); 
fcns.name{fcnNum}='nFANTOM100_destroyNXT'; fcns.calltype{fcnNum}='cdecl'; fcns.LHS{fcnNum}=[]; fcns.RHS{fcnNum}={'uint32', 'int32Ptr'};fcnNum=fcnNum+1;
% nFANTOM100_iFile nFANTOM100_iNXT_createFile ( nFANTOM100_iNXT nxtPtr , ViConstString fileName , ViStatus * status ); 
fcns.name{fcnNum}='nFANTOM100_iNXT_createFile'; fcns.calltype{fcnNum}='cdecl'; fcns.LHS{fcnNum}='uint32'; fcns.RHS{fcnNum}={'uint32', 'int8Ptr', 'int32Ptr'};fcnNum=fcnNum+1;
% void nFANTOM100_iNXT_destroyFile ( nFANTOM100_iNXT nxtPtr , nFANTOM100_iFile filePtr , ViStatus * status ); 
fcns.name{fcnNum}='nFANTOM100_iNXT_destroyFile'; fcns.calltype{fcnNum}='cdecl'; fcns.LHS{fcnNum}=[]; fcns.RHS{fcnNum}={'uint32', 'uint32', 'int32Ptr'};fcnNum=fcnNum+1;
% nFANTOM100_iFileIterator nFANTOM100_iNXT_createFileIterator ( nFANTOM100_iNXT nxtPtr , ViConstString fileNamePattern , ViStatus * status ); 
fcns.name{fcnNum}='nFANTOM100_iNXT_createFileIterator'; fcns.calltype{fcnNum}='cdecl'; fcns.LHS{fcnNum}='uint32'; fcns.RHS{fcnNum}={'uint32', 'int8Ptr', 'int32Ptr'};fcnNum=fcnNum+1;
% void nFANTOM100_iNXT_destroyFileIterator ( nFANTOM100_iNXT nxtPtr , nFANTOM100_iFileIterator fileIteratorPtr , ViStatus * status ); 
fcns.name{fcnNum}='nFANTOM100_iNXT_destroyFileIterator'; fcns.calltype{fcnNum}='cdecl'; fcns.LHS{fcnNum}=[]; fcns.RHS{fcnNum}={'uint32', 'uint32', 'int32Ptr'};fcnNum=fcnNum+1;
% nFANTOM100_iModule nFANTOM100_iNXT_createModule ( nFANTOM100_iNXT nxtPtr , ViConstString moduleName , ViUInt32 moduleID , ViUInt32 moduleSize , ViUInt32 ioMapSizeInBytes , ViStatus * status ); 
fcns.name{fcnNum}='nFANTOM100_iNXT_createModule'; fcns.calltype{fcnNum}='cdecl'; fcns.LHS{fcnNum}='uint32'; fcns.RHS{fcnNum}={'uint32', 'int8Ptr', 'uint32', 'uint32', 'uint32', 'int32Ptr'};fcnNum=fcnNum+1;
% void nFANTOM100_iNXT_destroyModule ( nFANTOM100_iNXT nxtPtr , nFANTOM100_iModule modulePtr , ViStatus * status ); 
fcns.name{fcnNum}='nFANTOM100_iNXT_destroyModule'; fcns.calltype{fcnNum}='cdecl'; fcns.LHS{fcnNum}=[]; fcns.RHS{fcnNum}={'uint32', 'uint32', 'int32Ptr'};fcnNum=fcnNum+1;
% nFANTOM100_iModuleIterator nFANTOM100_iNXT_createModuleIterator ( nFANTOM100_iNXT nxtPtr , ViConstString moduleNamePattern , ViStatus * status ); 
fcns.name{fcnNum}='nFANTOM100_iNXT_createModuleIterator'; fcns.calltype{fcnNum}='cdecl'; fcns.LHS{fcnNum}='uint32'; fcns.RHS{fcnNum}={'uint32', 'int8Ptr', 'int32Ptr'};fcnNum=fcnNum+1;
% void nFANTOM100_iNXT_destroyModuleIterator ( nFANTOM100_iNXT nxtPtr , nFANTOM100_iModuleIterator moduleIteratorPtr , ViStatus * status ); 
fcns.name{fcnNum}='nFANTOM100_iNXT_destroyModuleIterator'; fcns.calltype{fcnNum}='cdecl'; fcns.LHS{fcnNum}=[]; fcns.RHS{fcnNum}={'uint32', 'uint32', 'int32Ptr'};fcnNum=fcnNum+1;
% void nFANTOM100_iNXT_getFirmwareVersion ( nFANTOM100_iNXT nxtPtr , ViUInt8 * protocolVersionMajorPtr , ViUInt8 * protocolVersionMinorPtr , ViUInt8 * firmwareVersionMajorPtr , ViUInt8 * firmwareVersionMinorPtr , ViStatus * status ); 
fcns.name{fcnNum}='nFANTOM100_iNXT_getFirmwareVersion'; fcns.calltype{fcnNum}='cdecl'; fcns.LHS{fcnNum}=[]; fcns.RHS{fcnNum}={'uint32', 'uint8Ptr', 'uint8Ptr', 'uint8Ptr', 'uint8Ptr', 'int32Ptr'};fcnNum=fcnNum+1;
% ViUInt32 nFANTOM100_iNXT_sendDirectCommand ( nFANTOM100_iNXT nxtPtr , ViBoolean requireResponse , const ViByte commandBufferPtr [], ViUInt32 commandBufferSizeInBytes , ViPBuf responseBufferPtr , ViUInt32 responseBufferSizeInBytes , ViStatus * status ); 
fcns.name{fcnNum}='nFANTOM100_iNXT_sendDirectCommand'; fcns.calltype{fcnNum}='cdecl'; fcns.LHS{fcnNum}='uint32'; fcns.RHS{fcnNum}={'uint32', 'uint16', 'uint8Ptr', 'uint32', 'uint8Ptr', 'uint32', 'int32Ptr'};fcnNum=fcnNum+1;
% void nFANTOM100_iNXT_findDeviceInFirmwareDownloadMode ( ViChar resourceString [], ViStatus * status ); 
fcns.name{fcnNum}='nFANTOM100_iNXT_findDeviceInFirmwareDownloadMode'; fcns.calltype{fcnNum}='cdecl'; fcns.LHS{fcnNum}=[]; fcns.RHS{fcnNum}={'int8Ptr', 'int32Ptr'};fcnNum=fcnNum+1;
% void nFANTOM100_iNXT_downloadFirmware ( nFANTOM100_iNXT nxtPtr , const ViByte firmwareBufferPtr [], ViUInt32 firmwareBufferSize , ViStatus * status ); 
fcns.name{fcnNum}='nFANTOM100_iNXT_downloadFirmware'; fcns.calltype{fcnNum}='cdecl'; fcns.LHS{fcnNum}=[]; fcns.RHS{fcnNum}={'uint32', 'uint8Ptr', 'uint32', 'int32Ptr'};fcnNum=fcnNum+1;
% ViUInt32 nFANTOM100_iNXT_write ( nFANTOM100_iNXT nxtPtr , const ViByte bufferPtr [], ViUInt32 numberOfBytes , ViStatus * status ); 
fcns.name{fcnNum}='nFANTOM100_iNXT_write'; fcns.calltype{fcnNum}='cdecl'; fcns.LHS{fcnNum}='uint32'; fcns.RHS{fcnNum}={'uint32', 'uint8Ptr', 'uint32', 'int32Ptr'};fcnNum=fcnNum+1;
% ViUInt32 nFANTOM100_iNXT_read ( nFANTOM100_iNXT nxtPtr , ViPBuf bufferPtr , ViUInt32 numberOfBytes , ViStatus * status ); 
fcns.name{fcnNum}='nFANTOM100_iNXT_read'; fcns.calltype{fcnNum}='cdecl'; fcns.LHS{fcnNum}='uint32'; fcns.RHS{fcnNum}={'uint32', 'uint8Ptr', 'uint32', 'int32Ptr'};fcnNum=fcnNum+1;
% void nFANTOM100_iNXT_bootIntoFirmwareDownloadMode ( ViConstString resouceName , ViStatus * status ); 
fcns.name{fcnNum}='nFANTOM100_iNXT_bootIntoFirmwareDownloadMode'; fcns.calltype{fcnNum}='cdecl'; fcns.LHS{fcnNum}=[]; fcns.RHS{fcnNum}={'int8Ptr', 'int32Ptr'};fcnNum=fcnNum+1;
% void nFANTOM100_iNXT_setName ( nFANTOM100_iNXT nxtPtr , ViConstString newName , ViStatus * status ); 
fcns.name{fcnNum}='nFANTOM100_iNXT_setName'; fcns.calltype{fcnNum}='cdecl'; fcns.LHS{fcnNum}=[]; fcns.RHS{fcnNum}={'uint32', 'int8Ptr', 'int32Ptr'};fcnNum=fcnNum+1;
% void nFANTOM100_iNXT_getDeviceInfo ( nFANTOM100_iNXT nxtPtr , ViChar name [], ViByte bluetoothAddress [], ViUInt8 signalStrength [], ViUInt32 * availableFlash , ViStatus * status ); 
fcns.name{fcnNum}='nFANTOM100_iNXT_getDeviceInfo'; fcns.calltype{fcnNum}='cdecl'; fcns.LHS{fcnNum}=[]; fcns.RHS{fcnNum}={'uint32', 'int8Ptr', 'uint8Ptr', 'uint8Ptr', 'uint32Ptr', 'int32Ptr'};fcnNum=fcnNum+1;
% void nFANTOM100_iNXT_eraseUserFlash ( nFANTOM100_iNXT nxtPtr , ViStatus * status ); 
fcns.name{fcnNum}='nFANTOM100_iNXT_eraseUserFlash'; fcns.calltype{fcnNum}='cdecl'; fcns.LHS{fcnNum}=[]; fcns.RHS{fcnNum}={'uint32', 'int32Ptr'};fcnNum=fcnNum+1;
% ViUInt32 nFANTOM100_iNXT_pollAvailableLength ( nFANTOM100_iNXT nxtPtr , ViUInt32 bufferIndex , ViStatus * status ); 
fcns.name{fcnNum}='nFANTOM100_iNXT_pollAvailableLength'; fcns.calltype{fcnNum}='cdecl'; fcns.LHS{fcnNum}='uint32'; fcns.RHS{fcnNum}={'uint32', 'uint32', 'int32Ptr'};fcnNum=fcnNum+1;
% ViUInt32 nFANTOM100_iNXT_readBufferData ( nFANTOM100_iNXT nxtPtr , ViPBuf dataBuffer , ViUInt32 bufferIndex , ViUInt32 numberOfBytesToRead , ViStatus * status ); 
fcns.name{fcnNum}='nFANTOM100_iNXT_readBufferData'; fcns.calltype{fcnNum}='cdecl'; fcns.LHS{fcnNum}='uint32'; fcns.RHS{fcnNum}={'uint32', 'uint8Ptr', 'uint32', 'uint32', 'int32Ptr'};fcnNum=fcnNum+1;
% void nFANTOM100_iNXT_getResourceString ( nFANTOM100_iNXT nxtPtr , ViChar resourceString [], ViStatus * status ); 
fcns.name{fcnNum}='nFANTOM100_iNXT_getResourceString'; fcns.calltype{fcnNum}='cdecl'; fcns.LHS{fcnNum}=[]; fcns.RHS{fcnNum}={'uint32', 'int8Ptr', 'int32Ptr'};fcnNum=fcnNum+1;
% void nFANTOM100_iNXT_bluetoothFactoryReset ( nFANTOM100_iNXT nxtPtr , ViStatus * status ); 
fcns.name{fcnNum}='nFANTOM100_iNXT_bluetoothFactoryReset'; fcns.calltype{fcnNum}='cdecl'; fcns.LHS{fcnNum}=[]; fcns.RHS{fcnNum}={'uint32', 'int32Ptr'};fcnNum=fcnNum+1;
% void nFANTOM100_iNXTIterator_getName ( nFANTOM100_iNXTIterator iteratorPtr , char resourceName [], ViStatus * status ); 
fcns.name{fcnNum}='nFANTOM100_iNXTIterator_getName'; fcns.calltype{fcnNum}='cdecl'; fcns.LHS{fcnNum}=[]; fcns.RHS{fcnNum}={'uint32', 'cstring', 'int32Ptr'};fcnNum=fcnNum+1;
% void nFANTOM100_iNXTIterator_advance ( nFANTOM100_iNXTIterator iteratorPtr , ViStatus * status ); 
fcns.name{fcnNum}='nFANTOM100_iNXTIterator_advance'; fcns.calltype{fcnNum}='cdecl'; fcns.LHS{fcnNum}=[]; fcns.RHS{fcnNum}={'uint32', 'int32Ptr'};fcnNum=fcnNum+1;
% nFANTOM100_iNXT nFANTOM100_iNXTIterator_getNXT ( nFANTOM100_iNXTIterator iteratorPtr , ViStatus * status ); 
fcns.name{fcnNum}='nFANTOM100_iNXTIterator_getNXT'; fcns.calltype{fcnNum}='cdecl'; fcns.LHS{fcnNum}='uint32'; fcns.RHS{fcnNum}={'uint32', 'int32Ptr'};fcnNum=fcnNum+1;
methodinfo=fcns;