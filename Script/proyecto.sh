#!/bin/bash

run() {
    echo "Ejecutando el proyecto..."
    cd ..
    dotnet watch run --project MoogleServer
    cd script
}

compile_report() {
    echo "Compilando el informe..."
    cd ..
    cd Informe
    pdflatex Informe.tex
    cd ..
    cd Script
}

compile_slides() {
    echo "Compilando la Presentación..."
    cd ..
    cd Presentación
    pdflatex Presentación.tex
    cd ..
    cd Script
}

show_report() {
    cd ..
    cd Informe
    if [ ! -f "Informe.pdf" ]; then
        compile_report
    fi
    
    echo "Mostrando el informe..."
    xdg-open Informe.pdf
    cd ..
    cd Script
}

show_slides() {
    cd ..
    cd Presentación

    if [ ! -f "Presentación.pdf" ]; then
        compile_slides
    fi

    echo "Mostrando la Presentación..."
    xdg-open Presentación.pdf
    cd ..
    cd Script
}

clean() {
    echo "Eliminando archivos auxiliares..."
    cd ..
    cd Informe
    rm -f Informe.aux Informe.fls Informe.log Informe.fdb_latexmk Informe.out Informe.pdf Informe.synctex.gz Informe.toc
    cd ..
    cd Presentación
    rm -f Presentación.aux Presentación.fls Presentación.log Presentación.nav Presentación.out Presentación.pdf Presentación.synctex.gz Presentación.snm Presentación.toc Presentación.fdb_latexmk
    cd ..
    cd script
}

case "$1" in
    run)
        run
        ;;
    report)
        compile_report
        ;;
    slides)
        compile_slides
        ;;
    show_report)
        show_report
        ;;
    show_slides)
        show_slides
        ;;
    clean)
        clean
        ;;
    *)
        echo "Uso: ./proyecto.sh [opción]"
        echo "Opciones disponibles:"
        echo "run          - Ejecutar el proyecto"
        echo "report       - Compilar y generar el PDF del informe"
        echo "slides       - Compilar y generar el PDF de la presentación"
        echo "show_report  - Mostrar pdf del informe"
        echo "show_slides  - Mostrar pdf de la presentación"
        echo "clean        - Eliminar archivos auxiliares"
        ;;
esac

# En caso de que el pdf compile con errores repetir la compilación nuevamente