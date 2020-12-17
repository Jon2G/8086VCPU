using Gui.Compilador.Fases._4._Sintetizador;
using Gui.Compilador.Instrucciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.Compilador.Fases._3._Codigo_Intermedio
{
    public class CodigoMaquina
    {
        internal readonly CodeSegment CodeSegment;
        internal readonly StringBuilder Codigo;

        public CodigoMaquina(Sintesis sintetizado)
        {
            this.Codigo = new StringBuilder();
            this.CodeSegment = sintetizado.CodeSegment;

        }
        public void Generar()
        {
            foreach (Instruccion instruccion in this.CodeSegment.Instrucciones)
            {
                this.Codigo.Append(instruccion.Traduccion());
            }
        }
        //            public void Generar()
        //            {
        //                this.Codigo.AppendLine(".model small");
        //                this.Codigo.AppendLine(".stack");
        //                this.Codigo.AppendLine(".386");
        //                this.Codigo.AppendLine(".data");
        //                if (this.Programa.OperadoresAritmeticos || this.Programa.OperacionesLogicas || this.Programa.LeecturaNumerosDecimales || this.Programa.LeecturaNumerosEnteros)
        //                {
        //                    this.Codigo.AppendLine(";=========================================================");
        //                    this.Codigo.AppendLine(";VARIABLES TEMPORALES PARA LAS OPERACIONES ARITMETICAS");
        //                    this.Codigo.AppendLine(";PRIMER OPERADOR");
        //                    this.Codigo.AppendLine("SGN1 db 0");
        //                    this.Codigo.AppendLine("NUM1 db 0,0,0,0,0,0,0,0,0");
        //                    this.Codigo.AppendLine("DEC1 db 0,0,0,0,0,0,0,0,0 ");
        //                    this.Codigo.AppendLine(";SEGUNDO OPERADOR");
        //                    this.Codigo.AppendLine("SGN2 db 0");
        //                    this.Codigo.AppendLine("NUM2 db 0,0,0,0,0,0,0,0,0");
        //                    this.Codigo.AppendLine("DEC2 db 0,0,0,0,0,0,0,0,0");
        //                    this.Codigo.AppendLine(";VARIABLES DE RESULTADOS GLOBAL");
        //                    this.Codigo.AppendLine("SIGNOT     db 0");
        //                    this.Codigo.AppendLine("ENTEROST   db 0,0,0,0,0,0,0,0,0");
        //                    this.Codigo.AppendLine("DECIMALEST db 0,0,0,0,0,0,0,0,0");
        //                    this.Codigo.AppendLine(";AUXILIARES GLOBALES PARA LAS OPERACIONES");
        //                    this.Codigo.AppendLine("TEMP db 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0");
        //                    this.Codigo.AppendLine("AUX  dw 0");
        //                    this.Codigo.AppendLine("POSICION_DIV dw 0");
        //                    this.Codigo.AppendLine("OVERFLOWDIV dw 0");
        //                    this.Codigo.AppendLine("DIVISON dw 0,0,0");
        //                }
        //                if (this.Programa.LeecturaCadenas)
        //                {
        //                    this.Codigo.AppendLine(";=========================================================");
        //                    this.Codigo.AppendLine(";VARIABLES PARA LA LEECTURA DE CADENAS");
        //                    this.Codigo.AppendLine("CADENA DW 00");
        //                    this.Codigo.AppendLine("LONGUITUD_CADENA DW 0");
        //                    this.Codigo.AppendLine("BLOQUE_ACTUAL DB 0");
        //                    this.Codigo.AppendLine("ULTIMO_SEGMENTO DW 0");
        //                    this.Codigo.AppendLine("ERROR DB 'ERROR','$'");
        //                    this.Codigo.AppendLine("REALLOC DB 10,13,'REALLOC',10,13,'$'");
        //                    this.Codigo.AppendLine("ERROR_ALLOC_CADENA DB 10,13,'OCURRIO UN ERROR RESERVANDO MEMORIA PARA LA CADENA',10,13,'$'");
        //                }
        //                this.Codigo.AppendLine("FIN_CADENA DB '$'");
        //                if (this.Programa.OperacionesLogicas)
        //                {
        //                    this.Codigo.AppendLine(";=========================================================");
        //                    this.Codigo.AppendLine(";VARIABLES TEMPORALES PARA LAS OPERACIONES LOGICAS");
        //                    this.Codigo.AppendLine("R_COMPARADOR  db 0");
        //                    this.Codigo.AppendLine("CAD_TEMP1  dw 0");
        //                    this.Codigo.AppendLine("CAD_TEMP2  dw 0");
        //                    this.Codigo.AppendLine(";=========================================================");
        //                }
        //                if (this.Programa.LeecturaNumerosDecimales || this.Programa.LeecturaNumerosEnteros)
        //                {
        //                    this.Codigo.AppendLine("NUMERO_INVALIDO DB 'NUMERO INVALIDO!','$'");
        //                    this.Codigo.AppendLine("AUXILIAR  DB 0");
        //                }
        //                this.Codigo.AppendLine("OVERFLOW DB 'STACK OVERFLOW!','$'");
        //                this.Codigo.AppendLine("EXCEPCION_NO_CONTROLADA DB 'EXCEPCION NO CONTROLADA:','$'");
        //                this.Codigo.AppendLine(";VARIABLES DEPENDIENTES DE LA PROGRAMACION");
        //                this.Codigo.Append(this.Programa.SegmentoDeDatos.Traduccion());
        //                this.Codigo.AppendLine(".code");
        //                this.Codigo.AppendLine(";INICIA PROCEDIMIENTO PRINCIPAL");
        //                this.Codigo.AppendLine("begin proc FAR");
        //                this.Codigo.AppendLine("MOV AX,@data");
        //                this.Codigo.AppendLine("MOV DS,AX");
        //                this.Codigo.AppendLine(";================>[LIMPIAR PANTALLA]<================");
        //                this.Codigo.AppendLine(@"mov ax,0600h  ;ah 06(es un recorrido), al 00(pantalla completa)");
        //                this.Codigo.AppendLine(@"mov bh,01Fh    ;fondo blanco(7), sobre azul(1)");
        //                this.Codigo.AppendLine(@"mov cx,0000h  ;es la esquina superior izquierda reglon: columna");
        //                this.Codigo.AppendLine(@"mov dx,184Fh ;es la esquina inferior derecha reglon: columna");
        //                this.Codigo.AppendLine(@"int 10h  ;interrupcion que llama al BIOS");
        //                this.Codigo.AppendLine(";================>[MOVER CURSOR A 0]<================");
        //                this.Codigo.AppendLine("XOR DX,DX; dl Columna,dh fila");
        //                this.Codigo.AppendLine("MOV bh,0 ;PAGINA");
        //                this.Codigo.AppendLine(" mov ah, 02h ;mover cursor");
        //                this.Codigo.AppendLine("int  10h; ;servicio de video");
        //                this.Codigo.AppendLine(";================>[CODIGO GENERADO POR EL COMPILADOR]<================");
        //                foreach (Accion accion in this.Programa.Acciones)
        //                {
        //                    AgregarAccion(accion);
        //                }
        //                this.Codigo.AppendLine(";==============>[FIN CODIGO GENERADO POR EL COMPILADOR]<==============");
        //                this.Codigo.AppendLine("XOR AX,AX");
        //                this.Codigo.AppendLine("INT 16H");
        //                if (this.Programa.LeecturaCadenas)
        //                {
        //                    this.Codigo.AppendLine(";================>[LIBERAR TODA LA MEMORIA DE CADENAS]<================");
        //                    this.Codigo.Append(this.Programa.SegmentoDeDatos.Free());
        //                }
        //                this.Codigo.AppendLine("MOV AH,4CH");
        //                this.Codigo.AppendLine("INT 21H");
        //                this.Codigo.AppendLine("RET");
        //                this.Codigo.AppendLine("");

        //                #region Procedimientos
        //                this.Codigo.AppendLine(";==============>[PROCEDIMIENTOS INTERNOS GENERADOS POR EL COMPILADOR]<==============");
        //                if (this.Programa.UsarSuma)
        //                {
        //                    this.Codigo.AppendLine(@";====================================[SUMA]====================================        
        //    SUMA PROC NEAR
        //    ;LIMPIAR VARIABLES
        //    LEA DX,SIGNOT
        //    CALL LIMPIAR
        //    ;        
        //    ;COMPROBAR SI SE DEBERIA USAR LA RESTA EN SU LUGAR (-)(-) 
        //    MOV AL,SGN1
        //    CMP AL,SGN2
        //    JE  signos_iguales_suma
        //    JMP signos_diferentes_suma 

        //    signos_iguales_suma:
        //;;;;
        //MOV SIGNOT,AL
        //;;;
        //    CMP SGN1,0FFH
        //    JE sumar_y_negar  
        //    JNE sumar_con_normalidad

        //    signos_diferentes_suma:
        //    CMP SGN2,0FFH
        //    ;si el segundo es negativo pero el primero no,podemos restar tal cual :)
        //    JNE revisar_signo1_suma
        //    CALL RESTA
        //    RET
        //    revisar_signo1_suma:
        //    ;si el primero es negativo pero el segundo no,debemos invertir los operandos
        //    ;y restar normalmente...
        //        MOV SI,08H

        //            invierte_siguiente_suma:
        //            ;
        //            MOV AL,SGN1[SI]
        //            MOV AH,SGN2[SI] 

        //            MOV SGN2[SI],AL  
        //            MOV SGN1[SI],AH
        //            ;     
        //            DEC SI
        //            JNS invierte_siguiente_suma

        //        CALL RESTA

        //    RET
        //    sumar_y_negar:
        //    MOV SIGNOT,0FFH
        //    sumar_con_normalidad:   
        //            ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
        //            MOV SI,017D ;NUMERO MAS A LA IZQUIERDA             

        //                siguiente_sumando:


        //                    MOV AL,NUM2[SI] ;OP_1
        //                    ADD AL,NUM1[SI]
        //                    AAM
        //                    ;;===
        //                        ;

        //                        ADD AL,ENTEROST[SI]           
        //                        MOV ENTEROST[SI],AL


        //                        ADD AH,ENTEROST[SI-1]
        //                        MOV ENTEROST[SI-1],AH  

        //                        ;
        //                    ;;===
        //                    DEC SI
        //                    JNS siguiente_sumando                      


        //            ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;       

        //            ;Ajustar acarreos        

        //            MOV SI,018D ;Posicion del ultimo digito decimal ~  

        //            siguiente_acarreo:

        //            MOV AL,ENTEROST[SI]
        //            CMP AL,0AH
        //            JL sin_acarreo 
        //            AAM
        //            MOV ENTEROST[SI],AL
        //            ADD ENTEROST[SI-1H],AH

        //            sin_acarreo:  
        //            DEC SI
        //            JNS siguiente_acarreo
        //            RET

        //    SUMA ENDP");
        //                }
        //                if (this.Programa.UsarDivision)
        //                {
        //                    this.Programa.UsarResta = true;
        //                    this.Codigo.AppendLine(@";====================================[DIVIDE]====================================    
        //    DIVIDE PROC NEAR
        //        MOV DIVISON[0H],0
        //        MOV DIVISON[2H],0
        //        MOV DIVISON[4H],0

        //        MOV POSICION_DIV,0H     

        //        ;LOGICA DE SIGNOS :) BASTANTE SENCILLO AQUI
        //        MOV AL,SGN1
        //        MOV AH,SGN2
        //        IMUL AH
        //        PUSH AX ;SALVAR SIGNO

        //        ;PARTE_ENTERA
        //        divide_siguiente:

        //        CALL RESTA   
        //        MOV AL,SIGNOT
        //        CMP AL,0FFH
        //        JE parte_decimal

        //        ;  
        //            CMP POSICION_DIV,0H
        //            JE enteros_div  
        //            JA decimales_div_
        //                enteros_div:
        //                MOV SI,POSICION_DIV 
        //                ADD DIVISON[SI],01H

        //                JMP continuar_div
        //                decimales_div_:
        //                MOV SI,POSICION_DIV
        //                ADD DIVISON[SI],01H  
        //                ;JMP continuar_div

        //            continuar_div:    
        //            MOV SIGNOT,0H 
        //            LEA DX,SIGNOT
        //            CALL USAR_N1 
        //        ;
        //        JMP divide_siguiente


        //        parte_decimal:   
        //        ;MULTIPLICAR RESUIDO POR 10    
        //        MOV SI,018D    

        //        copy_next_div: 
        //        MOV AL,SGN1[SI]
        //        MOV TEMP[SI],AL
        //        DEC SI
        //        JNS copy_next_div

        //        LEA DX,SIGNOT
        //        CALL LIMPIAR

        //        MOV SGN1,0H
        //        MOV NUM1[0],0
        //        MOV NUM1[1],0
        //        MOV NUM1[2],0
        //        MOV NUM1[3],0 
        //        MOV NUM1[4],0 
        //        MOV NUM1[5],0 
        //        MOV NUM1[6],0 
        //        MOV NUM1[7],1 
        //        MOV NUM1[8],0 

        //        MOV DEC1[0],0
        //        MOV DEC1[1],0
        //        MOV DEC1[2],0
        //        MOV DEC1[3],0
        //        MOV DEC1[4],0
        //        MOV DEC1[5],0
        //        MOV DEC1[6],0
        //        MOV DEC1[7],0
        //        MOV DEC1[8],0

        //        CALL MULTIPLICA

        //        LEA DX,TEMP
        //        CALL USAR_N2 

        //        LEA DX,SIGNOT
        //        CALL USAR_N1

        //        ADD POSICION_DIV,2H
        //        CMP POSICION_DIV,06H
        //        JAE fin_division
        //        ;;;;;;;;;;;;;;;;;;REVISAR QUE EL NUM1 NO SEA YA CERO :V
        //        MOV SI,018D ;Posicion del ultimo digito decimal ~  
        //        revisar_siguiente_div:
        //            MOV AL,ENTEROST[SI]
        //            CMP AL,0H
        //            JNE divide_siguiente
        //        DEC SI
        //        CMP SI,0H
        //        JE fin_division
        //        JNE revisar_siguiente_div 
        //        ;;;;;;;;;;;;;;;;;;

        //        JMP divide_siguiente
        //        fin_division:
        //        ;COPIAR RESULTADO A T
        //            ;LIMPIAR VARIABLES
        //            LEA DX,SIGNOT
        //            CALL LIMPIAR

        //            MOV AX,DIVISON[0H]
        //            AAM
        //            MOV ENTEROST[7H],AH
        //            MOV ENTEROST[8H],AL

        //            MOV AX,DIVISON[2H]
        //            AAM
        //            MOV DECIMALEST[0H],AL
        //            MOV DECIMALEST[1H],AH 
        //            MOV AX,DIVISON[4H]
        //            AAM
        //            ADD DECIMALEST[0H],AH 
        //            ADD DECIMALEST[1H],AL

        //           POP AX 
        //           MOV SIGNOT,AL  ;RECUPERAR SINGO
        //        RET 

        //    DIVIDE ENDP");
        //                }
        //                if (this.Programa.UsarResta || this.Programa.UsarSuma)
        //                {
        //                    this.Codigo.AppendLine(@";====================================[RESTA]====================================        
        //    RESTA PROC NEAR  
        //    ;LIMPIAR OPERADORES 
        //    LEA DX,SIGNOT
        //    CALL LIMPIAR               
        //    ;;ACOMODAR LOS OPERANDOS DE MODO QUE EL MAYOR QUEDE EN NUM1
        //    MOV SI,0H
        //    acomodar_siguiente:
        //        MOV AL,NUM1[SI]
        //        MOV AH,NUM2[SI]
        //        CMP AL,AH
        //        JL invertir_operandos
        //        JA resta_es_positivo  
        //        INC SI
        //        CMP SI,018D
        //        JE acomodo_terminado
        //        JMP acomodar_siguiente 

        //    resta_es_positivo:
        //        MOV SIGNOT,01H
        //    JMP acomodo_terminado

        //    invertir_operandos:
        //    MOV SIGNOT,0FFH
        //    MOV SI,017D   ;~MOV SI,7H     
        //    invertir_siguiente:
        //    MOV AL,NUM1[SI]
        //    MOV AH,NUM2[SI]

        //    MOV NUM1[SI],AH
        //    MOV NUM2[SI],AL
        //    DEC SI
        //    JS acomodo_terminado 
        //    JMP invertir_siguiente 

        //    acomodo_terminado:
        //    ;;   
        //            ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
        //            MOV SI,017D ;NUMERO MAS A LA IZQUIERDA             

        //                siguiente_restando:


        //                    MOV AL,NUM1[SI] ;OP_1 
        //                    MOV AH,NUM2[SI]
        //                    SUB AL,AH
        //                    ;;===
        //                        ADD AL,ENTEROST[SI]           
        //                        MOV ENTEROST[SI],AL
        //                        ;
        //                    ;;===
        //                    DEC SI
        //                    JNS siguiente_restando                      


        //            ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;       

        //            ;Ajustar acarreos        

        //            MOV SI,017D  

        //            siguiente_acarreo_neg:

        //            MOV AL,ENTEROST[SI]
        //            CMP AL,0H 
        //            JS con_acarreo_neg
        //            JMP sin_acarreo_neg   
        //            ;;
        //            con_acarreo_neg: 
        //                NEG AL
        //                MOV AH,0AH
        //                SUB AH,AL
        //                MOV ENTEROST[SI],AH

        //                MOV AL,ENTEROST[SI-1]
        //                SUB AL,01H
        //                MOV ENTEROST[SI-1],AL
        //            ;;
        //            sin_acarreo_neg:  
        //            DEC SI
        //            JNS siguiente_acarreo_neg
        //            RET


        //    RESTA ENDP");
        //                }
        //                if (this.Programa.UsarMultiplicacion | this.Programa.UsarDivision)
        //                {
        //                    this.Codigo.AppendLine(@";====================================[MULTIPLICA]====================================
        //    MULTIPLICA PROC NEAR
        //    ;LIMPIAR VARIABLES
        //    LEA DX,SIGNOT
        //    CALL LIMPIAR
        //    ;LOGICA DE SIGNOS :) bastante sencillo aqui
        //        MOV AL,SGN1
        //        MOV AH,SGN2
        //        IMUL AH
        //        MOV SIGNOT,AL 

        //            MOV AUX,0H  
        //            ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
        //            MOV SI,12D ;NUMERO MAS A LA IZQUIERDA             

        //                siguiente_multiplicando:
        //                MOV DI,012D ;NUMERO MAS A LA IZQUIERDA
        //                ;MOV CX,7H 
        //                    MOV CX,016D

        //                SUB CX,AUX

        //                 multiplicar_operandos:
        //                    MOV AL,NUM2[SI] ;OP_1  
        //                    MOV AH,NUM1[DI]                 
        //                    MUL AH
        //                    AAM
        //                    ;;===
        //                    MOV BX,DI
        //                    MOV DI,CX

        //                        ;
        //                        MOV DX,DI
        //                        MOV DI,CX

        //                        ADD AL,ENTEROST[DI]           
        //                        MOV ENTEROST[DI],AL


        //                        ADD AH,ENTEROST[DI-1]
        //                        MOV ENTEROST[DI-1],AH  

        //                        MOV DI,DX
        //                        ;

        //                    ;;===
        //                    MOV DI,BX
        //                    DEC CX
        //                    ;;
        //                    DEC DI
        //                    JNZ multiplicar_operandos 
        //                    ;;;
        //                    INC AUX   
        //                    DEC CX
        //                    DEC SI
        //                    JNS siguiente_multiplicando                      


        //            ;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;       

        //            ;Ajustar acarreos        

        //            MOV SI,017D  

        //            siguiente_acarreo_multiplica:

        //            MOV AL,ENTEROST[SI]
        //            CMP AL,0AH
        //            JL sin_acarreo_multiplica 
        //            AAM
        //            MOV ENTEROST[SI],AL
        //            ADD ENTEROST[SI-1H],AH

        //            sin_acarreo_multiplica:  
        //            DEC SI
        //            JNS siguiente_acarreo_multiplica
        //            RET

        //    MULTIPLICA ENDP");
        //                }
        //                if (this.Programa.OperadoresAritmeticos || this.Programa.OperacionesLogicas || this.Programa.LeecturaNumerosEnteros || this.Programa.LeecturaNumerosDecimales)
        //                {
        //                    this.Codigo.AppendLine(@";====================================[USAR_N1]====================================    
        //    USAR_N1 PROC NEAR 
        //    MOV SI,018D ;DESPLAZAMIENTO POR EL CUAL SE ALCANZA LA ULTIMA CIFRA CONTANDO A CERO HASTA VARIABLE SIGNO
        //    ADD SI,DX 
        //    MOV DI,018D ;POSICION DEL ULTIMO NUMERO DEL DESTINO

        //    cpy_n1:
        //    MOV AL,[SI] 
        //    MOV SGN1[DI],AL

        //    DEC SI
        //    DEC DI
        //    JNS cpy_n1   

        //    RET    
        //    USAR_N1 ENDP 

        //;====================================[USAR_N2]====================================
        //    USAR_N2 PROC NEAR
        //    MOV SI,018D ;DESPLAZAMIENTO POR EL CUAL SE ALCANZA LA ULTIMA CIFRA CONTANDO A CERO HASTA VARIABLE SIGNO
        //    ADD SI,DX 
        //    MOV DI,018D ;POSICION DEL ULTIMO NUMERO DEL DESTINO

        //    cpy_n2:
        //    MOV AL,[SI] 
        //    MOV SGN2[DI],AL

        //    DEC SI
        //    DEC DI
        //    JNS cpy_n2   

        //    RET       
        //    USAR_N2 ENDP 
        //;====================================[SALVAR_TEMPORAL]====================================    
        //    SALVAR_TEMPORAL PROC NEAR
        //    MOV SI,018D ;Posicion del ultimo digito decimal
        //    MOV DI,018D
        //    ADD DI,DX 

        //    cpy_temp:
        //    MOV AL,SIGNOT[SI] 
        //    MOV [DI],AL

        //    DEC DI
        //    DEC SI
        //    JNS cpy_temp   

        //    RET       
        //    SALVAR_TEMPORAL ENDP

        //;====================================[LIMPIAR]====================================    
        //    LIMPIAR PROC NEAR  
        //        MOV SI,018D ;Posicion del ultimo digito decimal
        //        MOV CX,DX

        //        limpiar_siguienteT: 
        //        MOV DI,SI
        //        ADD DI,CX 
        //        MOV AL,0H
        //        MOV [DI],AL
        //        ;MOV SIGNOT[SI],0H  

        //        DEC SI
        //        JNS limpiar_siguienteT 
        //        RET
        //    LIMPIAR ENDP ");
        //                    if (this.Programa.ImprimeNumeros)
        //                    {
        //                        this.Codigo.AppendLine(@";====================================[MOSTRAR_RESULTADO]==================================== 
        //    MOSTRAR_RESULTADO PROC NEAR
        //        ;REVISAR EL SIGNO
        //        MOV AL,[DI] ;SIGNOT
        //        INC DI
        //        CMP AL,0FFH
        //        JNE no_imprime_signo
        //            ;IMPRIME SIGNO 
        //            MOV DL,'-'
        //            CALL IMPRIME_CARACTER
        //            ;
        //        no_imprime_signo:
        //        ;IMPRIME ENTEROS
        //        MOV SI,0FFFFH
        //            ;MIENTRAS SEA CERO A LA IZQUIERDA EVITAR
        //            siguiente_numero_entero:
        //            INC SI 
        //            ;;;;;;;;;;
        //            CMP SI,09H ;Ultima localidad del numero entero +1
        //            JE un_cero_entero
        //            ;;;;;;;;;; 

        //            ;;;;;;
        //            MOV CX,DI
        //            ADD CX,SI
        //            MOV BX,SI      
        //            MOV SI,CX     
        //            ;;;;;;
        //            MOV DL,[SI];ENTEROST[SI]  
        //            MOV SI,BX


        //            CMP DL,0H 
        //            JE siguiente_numero_entero
        //            ;YA NO ES CERO A LA IZQUIERDA
        //            CMP SI,09H ;SI SOLO TIENE DECIMALES - Ultima localidad del numero entero +1
        //            JE un_cero_entero
        //            ;     
        //                imprime_enteros:


        //                ;;;;;;
        //                MOV CX,DI
        //                ADD CX,SI
        //                MOV BX,SI      
        //                MOV SI,CX     
        //                ;;;;;;
        //                MOV DL,[SI];ENTEROST[SI]  
        //                MOV SI,BX

        //                ADD DL,30H
        //                CALL IMPRIME_CARACTER  
        //                INC SI
        //                CMP SI,08H ;Ultimo numero entero
        //                JA imprime_decimales
        //                JMP imprime_enteros
        //            ; 
        //            un_cero_entero: 
        //            MOV DL,30H
        //            CALL IMPRIME_CARACTER

        //           imprime_decimales:
        //           ;REVISAR SI TIENE DECIMALES PARA IMPRIMIR EL PUNTO
        //            MOV SI,08H ;Ultimo numero entero

        //            siguiente_cero_decimal:
        //            INC SI ;incremento en 1 para entrar en el campo de los enteros
        //            CMP SI,017D ;Ultimo numero decimal
        //            JA sin_decimales 


        //                ;;;;;;
        //                MOV CX,DI
        //                ADD CX,SI
        //                MOV BX,SI      
        //                MOV SI,CX     
        //                ;;;;;;
        //                MOV DL,[SI];DECIMALEST[SI]  
        //                MOV SI,BX


        //            CMP DL,0H
        //            JE siguiente_cero_decimal

        //            ;con decimales
        //            MOV DL,'.'
        //            CALL IMPRIME_CARACTER    
        //            MOV SI,08H ;Ultimo numero entero -1

        //            imprime_decimal_siguiente:
        //            INC SI


        //                ;;;;;;
        //                MOV CX,DI
        //                ADD CX,SI
        //                MOV BX,SI      
        //                MOV SI,CX     
        //                ;;;;;;
        //                MOV DL,[SI];DECIMALEST[SI]  
        //                MOV SI,BX


        //            ADD DL,30H
        //            CALL IMPRIME_CARACTER
        //            CMP SI,017D ;Ultimo numero decimal
        //            JNE imprime_decimal_siguiente


        //    sin_decimales:    
        //    RET 
        //    MOSTRAR_RESULTADO ENDP  
        //;====================================[IMPRIME_CARACTER]==================================== 
        //    IMPRIME_CARACTER PROC NEAR
        //       MOV AH,02H
        //       INT 21H          
        //    RET
        //    IMPRIME_CARACTER ENDP");
        //                    }
        //                }
        //                if (this.Programa.LeecturaCadenas)
        //                {
        //                    this.Codigo.AppendLine(@";==============>[ALOGAR_CADENA]<==============
        //        ALOGAR_CADENA PROC NEAR 

        //        ;MOV BLOQUE_ACTUAL,0AH ;INDICA EL BLOQUE EN EL QUE SE ESTA ESCRIBIENDO

        //        ;RESERVAR LA MEOMRIA PARA LEER LA CADENA (MALLOC)
        //        ;FUNCION AH,48H       

        //        ;PARAMETROS DE ENTRADA:
        //        ;BX - TAMANIO DE EL BLOQUE (EN PARRAFOS GRUPOS DE 16 BYTES)    

        //        ;PARAMETOS DE SALIDA:  
        //        ;SI NO HAY ERROR BANDERA DE ACARREO LIMPIA
        //        ;AX - APUNTA A LA DIRECCION DE INICIO DEL NUEVO BLOQUE RESERVADO

        //        ;SI HAY ERROR ACARREO CON VALOR,
        //        ;BX - TAMANIO MAXIMO DE MEMORIA DISPONIBLE
        //        ;AX CODIGO DE ERROR (7 O 8)

        //        MOV AH,48H
        //        XOR BX,BX
        //        MOV BL,BLOQUE_ACTUAL ;16*BLOQUE_ACTUAL BYTES DE INICIO
        //        INT 21H 
        //        MOV CADENA,AX 
        //        JC ERROR_ALLOC
        //        JMP FIN_ALOGAR_CADENA

        //ERROR_ALLOC:     
        //        MOV CADENA,DS
        //        XOR AX,AX
        //        MOV AL,BLOQUE_ACTUAL
        //        MOV AH,0FH
        //        MUL AH 

        //        MOV DI,CADENA
        //        ADD DI,AX
        //        MOV CX,ULTIMO_SEGMENTO
        //        ADD DI,CX
        //        MOV CADENA,DI

        //        ;MOV AH,09H
        //        ;LEA DX,ERROR_ALLOC_CADENA
        //        ;INT 21H    

        //        FIN_ALOGAR_CADENA:
        //        MOV CX,CADENA
        //        MOV ULTIMO_SEGMENTO,CX
        //        RET

        //    ALOGAR_CADENA ENDP");
        //                    this.Codigo.AppendLine(@"
        //        ;==============>[ALOGAR_CADENA]<==============
        //        LIBERAR_CADENA PROC NEAR ;(FREE)
        //            ;Funcion AH   49h  
        //            ;PARAMETROS DE ENTRADA: ES:0-DIRECCION BASE CERO DEL BLOQUE PARA LIBERAR
        //            ;PARAMETROS DE SALIDA: SI EL ACARREO ESTA ACTIVO AX CONTIENE EL CODIGO DE 
        //            ;ERROR (7,9)

        //            MOV AH,49H 
        //            MOV ES,BX
        //            INT 21H

        //            RET
        //            LIBERAR_CADENA ENDP ");
        //                    this.Codigo.AppendLine(@"LEER_CADENA PROC NEAR 
        //        XOR CX,CX



        //        MOV CADENA,DS
        //        XOR AX,AX
        //        MOV AL,BLOQUE_ACTUAL
        //        MOV AH,0FH
        //        MUL AH 

        //        MOV DI,CADENA
        //        ADD DI,AX
        //        MOV CX,ULTIMO_SEGMENTO
        //        ADD DI,CX
        //        MOV CADENA,DI
        //        XOR CX,CX 
        //        ;MOV AH,09H
        //        ;LEA DX,ERROR_ALLOC_CADENA
        //        ;INT 21H    
        //;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;LECTURA
        //AGREGAR_CARACTER:  
        //    MOV SI,CADENA
        //    MOV AH,08H
        //    INT 21H

        //    CMP AL,08H
        //    JE RETROCEDER
        //    CMP AL,0DH ;TECLA ENTER
        //    JE LEER_CADENA_SALIR

        //    MOV DI,SI
        //    ADD DI,CX 
        //    MOV [DI],AL 
        //    INC CX

        //    ;IMPRIMIR EL CARACTER EN PANTALLA
        //    MOV DL,AL
        //    MOV AH,02H
        //    INT 21H
        //    ;;REVISAR SI YA NOS ACABAMOS EL BLOQUE

        //    MOV dx,0 ; 
        //    MOV AX,CX ; 
        //    MOV BX,0FH
        //    DIV BX ;

        //    CMP AL,BLOQUE_ACTUAL 
        //    JA REALLOC_CADENA
        //    JMP AGREGAR_CARACTER
        //    ;;;;;;;;;;;;;;;;;;;;; 
        //            REALLOC_CADENA:
        //        ;MOV AH,09H
        //        ;LEA DX,REALLOC
        //        ;INT 21H
        //        ;;AUMENTAR EL BLOQUE Y RESERVAR UN PARRAFO MAS (REALLOC) 
        //        ;FUNCION AH,04AH    

        //        ;PARAMETROS ENTRADA
        //        ;ES - DIRECCION DEL BLOQUE A MODIFICAR
        //        ;BX NUEVO TAMANIO DE BLOQUE  

        //        ;PARAMETROS DE SALIDA
        //        ;SI HAY ERROR ACARREO CON VALOR,
        //        ;BX - TAMANIO MAXIMO DE MEMORIA DISPONIBLE
        //        ;AX CODIGO DE ERROR (7,8,9)        

        //            INC BLOQUE_ACTUAL

        //            MOV AH,04AH
        //            MOV ES,DI
        //            MOV BL,BLOQUE_ACTUAL
        //            INT 21H
        //            ;JC ERROR_REALLOC
        //            JMP AGREGAR_CARACTER
        //            ERROR_REALLOC:
        //            LEA DX,ERROR
        //            MOV AH,09H
        //            INT 21H 
        //    ;;;;;;;;;;;;;;;;;;;;;;     
        //    ;;;;;;;;;;;;;
        //    RETROCEDER:
        //    MOV AH,02H
        //    MOV DL,08H ;backspace 
        //    INT 21H 
        //    ;
        //    MOV AH,02H  
        //    MOV DL,00H ;caracter nulo para borra el caracter
        //    INT 21H
        //    ;
        //    MOV AH,02H  
        //    MOV DL,08H;backspace 
        //    INT 21H

        //    CMP CX,00H
        //    JE no_decrementar_posicion
        //    DEC CX                  

        //    no_decrementar_posicion:

        //    JMP AGREGAR_CARACTER
        //    ;;;;;;;;;;;;;
        //;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;    
        //        LEER_CADENA_SALIR:

        //        MOV AL,FIN_CADENA
        //        MOV DI,SI
        //        ADD DI,CX         
        //        MOV [DI],AL 
        //        INC CX

        //        MOV CX,CADENA
        //        MOV ULTIMO_SEGMENTO,CX

        //   RET      
        //   LEER_CADENA ENDP");
        //                }
        //                if (this.Programa.OperacionesLogicas)
        //                {
        //                    this.Codigo.AppendLine(@"IGUAL PROC NEAR

        //    MOV AL,SGN1
        //    MOV AH,SGN2
        //    CMP AL,AH
        //    JE signo_igual_igualdad 

        //    MOV R_COMPARADOR,0H
        //    JMP salir_igualdad               

        //signo_igual_igualdad:    
        //    MOV SI,0FFFFH
        //    ;  
        //    siguiente_igualdad:
        //        INC SI 
        //        CMP SI,0012 ;Posicion del ultimo digito decimal ~
        //        JA iguales

        //        MOV AL,NUM1[SI]
        //        MOV AH,NUM2[SI]
        //        CMP AL,AH
        //        JE siguiente_igualdad
        //        JNE diferente
        //    ;     

        //diferente:
        //    MOV R_COMPARADOR,0H
        //    JMP salir_igualdad
        //iguales:
        //    MOV R_COMPARADOR,1H

        //salir_igualdad:    
        //    RET    
        //IGUAL ENDP


        //MAYOR_QUE PROC NEAR

        //    MOV AL,SGN1
        //    MOV AH,SGN2
        //    CMP AL,AH
        //    JE signo_igual_mayorque
        //    JL menor_que_exit
        //    JC mayor_que_exit

        //    MOV R_COMPARADOR,0H
        //    JMP salir_mayorque               

        //signo_igual_mayorque:    
        //    MOV SI,0FFFFH
        //    ;  
        //    siguiente_mayorque:
        //        INC SI 
        //        CMP SI,018D ;Posicion del ultimo digito decimal ~
        //        JA iguales

        //        MOV AL,NUM1[SI]
        //        MOV AH,NUM2[SI]
        //        CMP AL,AH
        //        JA  mayor_que_exit    
        //        JL  menor_que_exit
        //        JE  siguiente_mayorque
        //        JMP siguiente_mayorque
        //    ;     

        //menor_que_exit:
        //    MOV R_COMPARADOR,0H
        //    JMP salir_igualdad
        //mayor_que_exit:
        //    MOV R_COMPARADOR,1H

        //salir_mayorque:    
        //    RET    
        //MAYOR_QUE ENDP

        //MENOR_QUE PROC NEAR

        //CALL MAYOR_QUE
        //MOV AL,R_COMPARADOR
        //CMP AL,01H
        //JE A_CERO 
        //JMP A_UNO

        //A_CERO:
        //MOV R_COMPARADOR,0H
        //JMP SALIR_MENOR_QUE

        //A_UNO:
        //MOV R_COMPARADOR,1H

        //SALIR_MENOR_QUE:
        //RET   
        //MENOR_QUE ENDP

        //MENOR_QUE_IGUAL PROC NEAR

        //CALL MENOR_QUE  
        //MOV AL,R_COMPARADOR
        //CMP AL,01H
        //JE VERDADERO
        //CALL IGUAL
        //MOV AL,R_COMPARADOR
        //CMP AL,01H
        //JE VERDADERO
        //JMP FALSO

        //VERDADERO:
        //MOV R_COMPARADOR,01H
        //JMP FIN_COMPARACION
        //FALSO:
        //MOV R_COMPARADOR,0H

        //FIN_COMPARACION: 
        //RET
        //MENOR_QUE_IGUAL ENDP
        //");
        //                    this.Codigo.AppendLine(@"IGUAL_CADENA PROC NEAR
        //    MOV R_COMPARADOR,1H

        //    MOV CX,0FFFFH
        //    XOR AX,AX
        //siguiente_iguales_cadena: 
        //    CMP AL,FIN_CADENA
        //    JE fin_iguales_cadena 

        //    INC CX

        //    MOV DI,CAD_TEMP1

        //    MOV SI,CAD_TEMP2

        //    ADD DI,CX
        //    ADD SI,CX

        //    MOV AL,[DI]
        //    MOV AH,[SI]

        //    CMP AL,AH
        //    JE siguiente_iguales_cadena
        //    JNE diferentes_iguales_cadena




        //diferentes_iguales_cadena:
        //MOV R_COMPARADOR,0H    

        //fin_iguales_cadena:
        //RET    
        //IGUAL_CADENA ENDP");
        //                    this.Codigo.AppendLine(@"DIFERENTE_CADENA PROC NEAR
        //CALL IGUAL_CADENA

        //MOV AL,R_COMPARADOR
        //CMP AL,01H
        //JE A_CERO_C 
        //JMP A_UNO_C

        //A_CERO_C:
        //MOV R_COMPARADOR,0H
        //JMP SALIR_DIFERENTE_C

        //A_UNO_C:
        //MOV R_COMPARADOR,1H

        //SALIR_DIFERENTE_C:
        //RET   
        //DIFERENTE_CADENA ENDP");
        //                }
        //                if (this.Programa.LeecturaNumerosDecimales)
        //                {
        //                    this.Codigo.AppendLine(@"LEER_DECIMAL PROC NEAR 
        //    XOR CX,CX ;CUENTA EL NUMERO DE ENTEROS
        //    MOV SI,DI
        //    MOV AH,01H
        //    INT 21H

        //    MOV AH,'-'          
        //    CMP AL,AH    
        //    JE lectura_entero_negativo
        //    MOV AH,'+'
        //    CMP AH,AL
        //    JE lectura_decimal_positivo
        //    JMP lectura_decimal_positivo_usgn

        //    lectura_decimal_positivo:
        //    MOV DL,01H 
        //    MOV [DI],DL
        //    JMP lectura_numeros

        //    lectura_decimal_positivo_usgn:
        //    MOV DL,01H
        //    MOV [DI],DL
        //    INC DI  
        //    INC CX
        //    JMP validar_numero_entero
        //lectura_entero_negativo:
        //    MOV DL,0FFH
        //    MOV AL,DL 
        //    MOV [DI],AL   

        //    JMP lectura_numeros
        //    back_space_lectura_numeros_decimales:    
        //    DEC CX
        //    CMP CX,00H
        //    JE lectura_numeros
        //    DEC DI
        //    MOV [DI],00H
        //    DEC CX
        //    DEC DI

        //    ;NUMEROS
        //    lectura_numeros:
        //    INC CX
        //    INC DI 
        //    CMP CX,0BH 
        //    JAE overflow_numerico 

        //    MOV AH,01H
        //    INT 21H       

        //    CMP AL,0DH
        //    JE fin_lectura_entero 
        //    CMP AL,08H
        //    JE back_space_lectura_numeros_decimales 

        //    CMP AL,2EH ;'.' 
        //    JE lectura_numeros_decimales

        //    validar_numero_entero:
        //    CMP AL,30H
        //    JL numero_invalido_enteros
        //    CMP AL,39H
        //    JA numero_invalido_enteros   

        //    SUB AL,30H
        //    MOV [DI],AL
        //    JMP lectura_numeros

        //    lectura_numeros_decimales:
        //    MOV DI,SI
        //    ADD DI,0AH

        //    lectura_numero_decimal:
        //    CMP AUXILIAR,09H 
        //    JA overflow_numerico

        //    MOV AH,01H
        //    INT 21H

        //    CMP AL,0DH
        //    JE fin_lectura_entero  
        //    CMP AL,08H
        //    JE back_space_decimal_lec
        //    CMP AL,30H
        //    JL numero_invalido_enteros
        //    CMP AL,39H
        //    JA numero_invalido_enteros   

        //    SUB AL,30H
        //    MOV [DI],AL

        //    INC AUXILIAR
        //    INC DI 

        //   JMP lectura_numero_decimal

        //    back_space_decimal_lec:
        //    CMP AUXILIAR,00H
        //    JE lectura_numeros_regresar_enteros
        //    DEC AUXILIAR
        //    DEC DI
        //    MOV [DI],00H ;BORRAR 
        //    MOV DL,00H
        //    MOV AH,02H
        //    INT 21H  
        //    MOV DL,08H
        //    MOV AH,02H
        //    INT 21H
        //    JMP lectura_numero_decimal

        //    lectura_numeros_regresar_enteros:
        //    MOV DL,00H
        //    MOV AH,02H
        //    INT 21H  
        //    MOV DL,08H
        //    MOV AH,02H
        //    INT 21H
        //    INT 21H
        //    MOV DL,00H
        //    INT 21H
        //    MOV DL,08H
        //    INT 21H
        //    DEC CX
        //    MOV DI,SI
        //    ADD DI,CX 
        //    INC DI
        //    INC CX
        //    JMP back_space_lectura_numeros_decimales

        //fin_lectura_entero:
        //;AJUSTAR LA PARTE DECIMAL
        //DEC CX
        //CMP CX,09H;04H
        //JL  ajuste_entero_necesario
        //JMP sin_ajuste_entero
        //ajuste_entero_necesario:  

        //MOV AL,CL
        //MOV AH,09H;04H
        //SUB AH,AL
        //XOR DX,DX
        //MOV DL,AH

        //MOV DI,SI
        //INC DI
        //MOV BX,DI

        //DEC CX
        //siguiente_ajuste_entero:
        //MOV DI,BX
        //ADD DI,CX

        //MOV AL,[DI]
        //MOV AH,0H
        //MOV [DI],AH
        //ADD DI,DX
        //MOV [DI],AL


        //DEC CX
        //JNS siguiente_ajuste_entero

        //sin_ajuste_entero:
        //RET    
        //overflow_numerico: 
        //LEA DI,OVERFLOW  
        //CALL EXCEPCION  
        //numero_invalido_enteros:
        //LEA DI,NUMERO_INVALIDO
        //CALL EXCEPCION

        //RET               
        //LEER_DECIMAL ENDP");

        //                }
        //                if (this.Programa.LeecturaNumerosEnteros)
        //                {
        //                    this.Codigo.AppendLine(@"
        //ES_NUMERO PROC NEAR

        //    CMP AL,30H
        //    JL  numero_invalido_enteros3
        //    CMP AL,39H
        //    JA numero_invalido_enteros3 

        //    SUB AL,30H 
        //    RET
        //    numero_invalido_enteros3:
        //    MOV AL,'$'

        //RET    
        //ENDP
        //LEER_ENTERO PROC NEAR 

        //    ;LIMPIAR VARIABLES
        //    MOV DX,DI
        //    CALL LIMPIAR

        //    MOV SI,DI
        //    XOR CX,CX

        //    MOV DL,01H
        //    MOV [DI],DL

        //    MOV AH,01H
        //    INT 21H

        //    ;REVISAR SIGNO
        //    MOV AH,'-'
        //    CMP AH,AL
        //    JE lectura_entero_negativo2
        //    MOV AH,'+'
        //    CMP AH,AL
        //    JE lectura_entero_positivo2
        //    JMP validar_numero

        //    lectura_entero_negativo2:
        //    MOV DL,0FFH
        //    MOV [DI],DL
        //    JMP siguiente_entero

        //    lectura_entero_positivo2:
        //    MOV DL,01H
        //    MOV [DI],DL
        //    JMP siguiente_entero

        //    siguiente_entero:
        //    MOV AH,01H
        //    INT 21H    
        //    ;;;;;;;;;
        //        validar_numero: 
        //        CMP CX,09H
        //        JAE overflow_numerico2
        //            CMP AL,08H
        //            JE retroceso_entero 
        //            CMP AL,0DH
        //            JE fin_ingreso_entero
        //        CALL ES_NUMERO
        //        MOV AH,'$'
        //        CMP AL,AH
        //        JNE salvar_numero
        //        JMP numero_invalido_enteros2   
        //    ;;;;;;;;; 
        //    salvar_numero:
        //    INC DI
        //    INC CX
        //    MOV [DI],AL
        //    JMP siguiente_entero

        //    retroceso_entero:
        //    MOV DL,00H
        //    MOV AH,02H
        //    INT 21H
        //    MOV DL,8H
        //    INT 21H    

        //    CMP CX,0H
        //    JE siguiente_entero
        //    DEC CX
        //    MOV DL,0H
        //    MOV [DI],DL
        //    DEC DI
        //    JMP siguiente_entero 

        //fin_ingreso_entero:
        //    ;AJUSTE
        //    CMP CX,00H
        //    JE entero_ingreso_vacio
        //    CMP CX,09H
        //    JL ajustar_entero

        //    ajustar_entero: 
        //    MOV AX,SI ;SALVAR DIRECCION


        //    MOV DI,SI
        //    ADD DI,CX ;FUENTE 

        //    ADD SI,09H ;DESTINO


        //    ajusta_siguiente_digito_entero:

        //        MOV AL,[DI]
        //        MOV [SI],AL
        //        MOV DL,0H
        //        MOV [DI],DL

        //        DEC SI
        //        DEC DI   

        //    DEC CX    
        //    CMP CX,00H
        //    JE salir_ajuste_entero
        //    JMP ajusta_siguiente_digito_entero
        //entero_ingreso_vacio:
        //salir_ajuste_entero:    
        //RET       
        //overflow_numerico2: 
        //LEA DI,OVERFLOW  
        //CALL EXCEPCION  
        //numero_invalido_enteros2:
        //LEA DI,NUMERO_INVALIDO
        //CALL EXCEPCION               
        //RET               
        //LEER_ENTERO ENDP");
        //                }
        //                if (this.Programa.LeecturaNumerosDecimales || this.Programa.LeecturaNumerosEnteros)
        //                {
        //                    this.Codigo.AppendLine(@"EXCEPCION PROC NEAR

        //MOV AH, 06h    ; Scroll up function
        //XOR AL, AL     ; Clear entire screen
        //XOR CX, CX     ; Upper left corner CH=row, CL=column
        //MOV DX, 184FH  ; lower right corner DH=row, DL=column 
        //MOV BH, 4Eh    ; YellowOnBlue
        //INT 10H

        //MOV DL,1AH
        //MOV DH,0AH
        //MOV bh,0 ;PAGINA
        //mov ah, 02h ;mover cursor
        //int  10h; ;servicio de video 


        //LEA DX,EXCEPCION_NO_CONTROLADA
        //MOV AH,09H
        //INT 21H

        //MOV DL,1AH
        //MOV DH,0CH
        //MOV bh,0 ;PAGINA
        //mov ah, 02h ;mover cursor
        //int  10h; ;servicio de video 

        //MOV DX,DI
        //MOV AH,09H
        //INT 21H         

        //XOR AX,AX
        //INT 16H

        //MOV AH,4CH
        //INT 21H

        //EXCEPCION ENDP");
        //                }
        //                if (this.Programa.ImprimeCadenas)
        //                {
        //                    this.Codigo.AppendLine(@"IMPRIME_CADENA PROC NEAR

        //    MOV DI,DX

        //    siguiente_caracter_impresion2:
        //    MOV DL,[DI]
        //    CMP DL,015H ;ALIAS IMPRIMIBLE PARA $
        //    JE imprime_signo_pesos
        //    CMP DL,'$'
        //    JE salir_IMPRIME_CADENA

        //    MOV AH,02H
        //    INT 21H

        //    siguiente_caracter_impresion:
        //    INC DI
        //    JMP siguiente_caracter_impresion2


        //    imprime_signo_pesos:
        //    MOV DL,'$'
        //    MOV AH,02H
        //    INT 21H


        //    JMP siguiente_caracter_impresion

        //salir_IMPRIME_CADENA:
        //RET    
        //IMPRIME_CADENA ENDP");
        //                }
        //                #endregion
        //                this.Codigo.AppendLine("begin endp");
        //                this.Codigo.AppendLine("end begin");
        //                Clipboard.SetText(this.Codigo.ToString().Replace(".386", "").Replace("begin endp", ""));
        //            }
        //            private void AgregarAccion(Accion accion)
        //            {
        //                if (this.BloquesPorCerrar.Any())
        //                {
        //                    if (this.Programa.Acciones.Where(x => !(x is IBloque)).LastOrDefault() == accion)
        //                    {
        //                        if (this.BloquesPorCerrar.Count == 1)
        //                        {
        //                            IBloque block = this.BloquesPorCerrar.Dequeue();
        //                            AgregarAccion(accion);
        //                            this.Codigo.Append(block.CerrarBloque());
        //                        }
        //                        else
        //                        {
        //                            CerrarBLoquesPendientes();
        //                            AgregarAccion(accion);
        //                        }
        //                        return;
        //                    }
        //                    if (accion.LineaDocumento.LineNumber >= this.BloquesPorCerrar.Peek().FinBloque.LineNumber)
        //                    {
        //                        this.Codigo.Append(this.BloquesPorCerrar.Dequeue().CerrarBloque());
        //                    }
        //                }
        //                this.Codigo.Append(accion.Traduccion());
        //                if (accion is IBloque bloque)
        //                {
        //                    BloquesPorCerrar.Enqueue(bloque);
        //                }
        //            }
        //            private void CerrarBLoquesPendientes()
        //            {
        //                while (this.BloquesPorCerrar.Any())
        //                {
        //                    this.Codigo.Append(this.BloquesPorCerrar.Dequeue().CerrarBloque());
        //                }
        //            }

    }
}
