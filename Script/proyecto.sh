#!/bin/bash

# Funciones
run() {
    echo "Ejecutando el proyecto..."
    cd ..
    dotnet watch run --project MoogleServer
    cd script
}

report() {
    echo "Compilando y generando el PDF del informe..."
    cd ..
    cd Informe
    pdflatex Informe.tex
    cd ..
    cd script
}

slides() {
    echo "Compilando y generando el PDF de la presentación..."
    cd ..
    cd Presentacion
    #chmod u+w Presentacion_Moogle.tex
    pdflatex Presentacion_Moogle.tex
    cd ..
    cd script
}

show_report() {
    cd ..
    cd Informe
    if [ ! -f Informe.pdf ]; then
        echo "Compilando y generando el PDF del informe..."
        pdflatex Informe.tex
    fi
    echo "Visualizando el informe..."
    if [ -z "$1"]; then
        xdg-open Informe.pdf
    else
        $1 Informe.pdf
        return 1
    fi
    cd ..
    cd script
}

show_slides() {
    cd ..
    cd Presentacion
    if [ ! -f Presentacion_Moogle.pdf]; then
        slides
    fi
    echo "Visualizando la presentación..."
    if [ -z "$1"]; then
        xdg-open Presentacion_Moogle.pdf
    else
        $1 Presentacion_Moogle.pdf
        return 1
    fi
    cd ..
    cd script
}

clean() {
    echo "Eliminando los ficheros auxiliares..."
    cd ..
    cd Informe
    find . ! -name '*.tex' ! -name '*.png' ! -name '*.jpg' -type f -delete
    cd ..
    cd Presentacion
    find . ! -name '*.tex' -type f -delete
    cd ..
    cd script
}

Guide() {
    echo " "
    echo "1) El comando <run> te permite ejecutar el Moogle"
    echo " "
    echo "2) El comando <report> te permite Compilar y generar el PDF del Informe del Moogle (Latex) que se encuentra en la carpeta Informe"
    echo " "
    echo "3) El comando <slides> te permite Compilar y generar el PDF de la Presentacion del Moogle (Latex) que se encuentra en la carpeta Presentacion"
    echo " "
    echo "4) El comando <show_report> te permite mostrar el PDF Informe del Moogle, y si este no ha sido generado, lo genera y luego lo muestra"
    echo "    - Este comando tiene la utilidad de que puede ser ejecutado con el lector de PDF que desee, solo basta con pasarselo como parametro de la siguiente forma: "
    echo "    <script_name.sh> <show_report> <lector_a_usar>"
    echo "    En el caso de que no le pase ningun lector como parametro, se abrira con un lector de PDF por defecto"
    echo " "
    echo "5) El comando <show_slides> te permite mostrar el PDF Presentacion del Moogle, y si este no ha sido generado, lo genera y luego lo muestra"
    echo "    - Este comando tiene la utilidad de que puede ser ejecutado con el lector de PDF que desee, solo basta con pasarselo como parametro de la siguiente forma: "
    echo "    <script_name.sh> <show_slides> <lector_a_usar>"
    echo "    En el caso de que no le pase ningun lector como parametro, se abrira con un lector de PDF por defecto"
    echo " "
    echo "6) El comando <clean> te permite borrar los archivos auxiliares que se crean cuando se compilan y se general los PDF del Informe y la Presentacion"
    echo " "
    echo "7) El comando <ZIP> te permite comprimir el Informe,la Presentacion y la carpeta del Proyecto, para usarlo solo debe seguir con las instrucciones que se muestran al darle al comando"
    echo " "
}

# opciones
OPTIONS="run report slides show_report show_slides clean ZIP Guide"

# Ejecución del script

OPT=$1
if [ "$OPT" = "" ]; then
    echo "Sintaxys se uso: $0 <opcion>"
    echo ""
    echo "Opciones:"
    for i in $OPTIONS; do
        echo "  $i"
    done
    exit 1
fi

"$@"
