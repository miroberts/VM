        ;; ADR LDR ADD and SWI
FIRST   .WORD 2
SECOND  .WORD 3
    .BYTE \0
        ADR R3 FIRST            ; FIRST = 2
        LDR R1 [R3]
        ADR R3 SECOND           ; SECOND = 3
        LDR R2 [R3]
		;EOR R1 R2    
        ADD R0 R1 R2
        SWI 0