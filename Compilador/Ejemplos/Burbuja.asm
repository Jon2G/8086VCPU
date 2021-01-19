db temporales 00D,00D,00D,00D,00D  
db vector     0FH,0EH,0DH,0CH,0BH,0AH,9D,8D,7D,6D,5D,4D,3D,2D,01D,0FFH  

begin

MOV SI, 07D
;==========>CALCULAR TAMAÑO DEL VECTOR<==========
    calcular_tamano:
        MOV AX,[SI]
        CMP AX,0FFH 
        JE  fin_alcanzado 
        ADD SI,01D
    JMP calcular_tamano
;================================================
    fin_alcanzado:
    ;SUB SI,5D
    MOV [0D],SI ;salvar la longuitud
;==========>ORDENAMIENTO DE LA BURBUJA<==========
   
    SUB SI,2D   ;longuitud - 2   
    MOV AX,SI
    MOV [04D],AL ;salvar el valor "longuitud-2"
    
;----------------  
;|longuitud   = [0] |
;|i           = [1] |
;|j           = [2] |
;|temporal    = [3] |
;|longuitud-2 = [4] | 
;----------------
    for_1:
        MOV CL,07D
        MOV [02D],CL   ;j=7
        JMP iteracion_1
        
        incremento_1:
            MOV DI,2D
            MOV BX,[DI] ;leer j
            ADD BX,01D  ;j++
            MOV [DI],BX ;salvar el valor de j
        
        iteracion_1:
        ;
            for_2:
                MOV CL,07D
                MOV [01D],CL ;i=7
                JMP iteracion_2
                
                incremento_2:
                    MOV BH,0H
                    MOV BL,[01D] ;leer i
                    ADD BX,01D  ;i++ 
                    MOV [01D],BL ;salvar el valor de i
                    
                iteracion_2:
                ;   
                    MOV AH,0H
                    MOV AL,[01D]
                    MOV DI,AX ;valor de i 
                    
                    MOV AH,0H
                    MOV AL,[02D]
                    MOV SI,AX ;valor de j
                    
                    MOV AL,[DI]  ;vector[i]
                    ADD DI,1D
                    MOV BL,[DI]  ;vector[i+1] 
                    
                    CMP AL,BL    ;Si vector[i]>vector[i+1]  
                    JA  intercambiar
                    JMP condicional_2 ;else continuar
                    
                    intercambiar:                       
                        MOV BL,[DI] ;obtener vector[i+1]
                        MOV [03D],BL  ;salvar vector[i+1] en temporal
                                         
                        MOV BL,[SI] ;obtener vector[j] 
                        MOV [DI],BL ;mover vector[j] a vector[i+1]
                        
                        MOV AH,0H
                        MOV AL,[03D]
                        MOV [SI],AL ;mover el temporal a vector[i]         
                
                ;
                
                condicional_2: ;i<=longuitud-2
                    MOV AL,[04D] ;leer longuitud-2
                    MOV BL,[01D] ;leer i
                        
                    CMP BL,AL   ; ;Comparar longuitud<=i  
                    JLE incremento_2 ;incrementar y repetir
                
        ;
        
        condicional_1:;j<=longuitud-2
            MOV AL,[04D] ;leer longuitud-2   
            MOV BL,[02D] ;leer j
        
            CMP BL,AL   ;Comparar longuitud<=j
            JLE incremento_1 ; incrementar y repetir    
    
;================================================

RET