#!/bin/bash

run() {
    echo "Ejecutando el proyecto..."
    dotnet watch run --project ../MoogleServer
}

compile_report() {
    echo "Compilando el informe..."
    cd ../informe
    pdflatex Informe.tex
    cd ../script
}

compile_slides() {
    echo "Compilando la presentación..."
    cd ../presentación
    pdflatex Presentación.tex
    cd ../script
}

show_report() {
    cd ../informe
    if [ ! -f "Informe.pdf" ]; then
        compile_report
    fi
    echo "Mostrando el informe..."
    xdg-open Informe.pdf
    cd ../script
}

show_slides() {
    cd ../presentación

    if [ ! -f "Presentación.pdf" ]; then
        compile_slides
    fi
    echo "Mostrando la Presentación..."
    xdg-open Presentación.pdf
    cd ../script
}

clean() {
    echo "Eliminando archivos auxiliares..."
    cd ../informe
    rm -f Informe.aux Informe.fls Informe.log Informe.fdb_latexmk Informe.out Informe.pdf Informe.synctex.gz Informe.toc
    cd ../presentación
    rm -f Presentación.aux Presentación.fls Presentación.log Presentación.nav Presentación.out Presentación.pdf Presentación.synctex.gz Presentación.snm Presentación.toc Presentación.fdb_latexmk
    cd ../script
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