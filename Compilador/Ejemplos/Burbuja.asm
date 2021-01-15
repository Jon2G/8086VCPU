db vector 01H,07h,02h,08h,03h,09h,04h,0Ah,05h,0BH,06h,0CH,0FFH
db longitud 0H


begin
;=>>>>>>>>>>>[CALCULEMOS LA LONGUITUD DEL VECTOR....]<<<<<<<<<<<=
MOV SI,0H
MOV BX,0H ;LA BASE EMPIEZA EN CERO QUE ES DONDE ESTA ALOJADO EL INICIO DEL VECTOR

	AunNoEsFin:
		MOV AH,[BX+SI]
		CMP AH,0FFH   ;En este caso nosotros determinamos para el ejemplo el valor 'FF' como fin de cadena
		JE  FinCadena
		ADD SI,01D    ;Debemos pasar a la siguiente localidad incrementado el Indice fuente
		JMP AunNoEsFin

	FinCadena:
		;LA LONGUITUD DEL VECTOR-1 AH QUEDADO ALMACENADA EN SI
		MOV DX,SI ;ESCRIBIR EN DX LA LONGUITUD
			
;=>>>>>>>>>>>[ORDENAMIENTO DE LA BURBUJA....]<<<<<<<<<<<=
MOV DI,0D
	repetir_1:
		
		MOV SI,0D
		
		repetir_2:
		
		MOV AH,[SI]      ;arr[j]
		MOV BX,1D
		MOV AL,[BX+SI]   ;arr[j+1]
		
		CMP AH,AL 		 ;if (arr[j]>arr[j+1])
		JA mayor_que
		JMP else_mayor_que
			
			mayor_que:
			
				MOV CL,[SI]  ;salvar arr[j] el CL
		    	;MOV [SI],AL  ;Mover arr[j+1] a arr[j] 
		    	MOV AL,CL    
		    	;MOV [BX+SI],AH  ;Mover arr[j] a  arr[j+1]
		    	
			else_mayor_que:
			MOV CX,SI     ;MOVER A CX EL VALOR DE (I) PARA COMPARAR SI ES IGUAL A LA LONGUITUD DE CADENA
			SUB CX,1D     ;RESTAR 1 PARA COMPARAR EL VALOR DE  longuitd-1 con (I)
			CMP CX,SI     
			JL repetir_2
		
		
	MOV CX,DI	;MOVER A CX EL VALOR DE (J) PARA COMPARAR SI ES IGUAL A LA LONGUITUD DE CADENA
	CMP CX,DX
	JAE ordenado
	ADD DI,1D   ;J++
	JMP repetir_1
ordenado:

RET