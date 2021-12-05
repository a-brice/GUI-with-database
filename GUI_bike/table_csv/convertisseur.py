# -*- coding: utf-8 -*-
"""
Created on Wed Apr 21 22:22:06 2021

@author: brice
"""

import pandas as pd

files = ["individu", "modele", "fournisseur"]
files += [ "piece", "boutique", "programme"]
files += ["delivrer", "grandeur" , "associe" ]
files += ["commande","adhere", "compose"]

sqlfile = "insertion.sql"

stc = ""
for file in files:
    
    with open(file + ".csv",'r') as f:
        i = 0
        for line in f:
            if i == 0:
                i+=1
            else : 
                stc += f"insert into {file} values " 
                s  = "','".join([i.replace("'", "`") for i in line.split(';') if i]).replace('\n','')
                stc += "('" + s + "');\n"
        stc +='\n'
            
with open(sqlfile,'w') as f:
    f.write(stc)   

