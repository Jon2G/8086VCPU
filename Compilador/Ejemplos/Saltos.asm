db 1d,2d,3d,4d,5d,6d,7h,8d
begin
MOV AX,02D
MOV BX,7D

incrementa:

ADD AX,01D
CMP AX,BX
JA adios
JMP incrementa

adios:
RET
