ADD AX,00011B
ADD CX,10010B
ADD AL,6d
ADD AX,-2d
ADD AL,0111b
ADD CL,5d
ADD DL,2D
ADD CL,7d
ADD CL,-7d
ADD al,5d
ADD bl,3d

ADD AL,CL
ADD AH,AL
ADD CX,AX
ADD AX,DX

;ADD AH,AX
;ADD AX,AH


ADD AX,[0d]
ADD AX,[BX]
ADD AX,[BX+SI]
ADD AX,[BX+DI]

ADD AX,[BX+ SI]
ADD AX,[BX + DI]

;ADD AX,[BX]
;ADD AX,[AX]         
;ADD AX,[BX]
;ADD AX,[CX]
;ADD AX,[DX]

ADD AX,[SI]    
ADD AX,[DI]

NOT AX

NOT AX
OR AX,AX
NOR AX,AX 
XOR AX,AX
XNOR AX,AX 
AND AX,AX
NAND AX,AX