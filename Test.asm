        ;; ADD ADR DIV LDR MUL SUB and SWI
    COUNT  .WORD 1                                                              
	INDEX  .WORD 3       
	RASN   .WORD 4
	ASCI   .BYTE 'k'
	ASCII  .BYTE \0
		   MVI R1 COUNT
 	       MVI R2 INDEX
           ADD R0 R1 R2		; ADD R0 = 4 R1= 1 R2 = 3
		   MVI R3 RASN		; test stack managment R3 = 4
		   LDR R2 [R3]		; LDR R2 = 4
		   DIV R0 R2 R0		; DIV R0 = 1
		   SUB R0 R1 R0		; SUB R0 = 0
		   MOV R0 R1		; MOV R0 = 1
		   MUL R0 R1 R2		; MUL R0 = 3
		   ADR R3 INDEX		; ADR R3 = 3
		   STR R1 R2		; STR Memory(1,page) = 4
							; SWI 0