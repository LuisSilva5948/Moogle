#!/bin/bash

cd..

run() {
    echo "Ejecutando el proyecto..."
    dotnet watch run --project MoogleServer
}

compile_report() {
    echo "Compilando el informe..."
    cd Informe
    pdflatex informe.tex
    pdflatex informe.tex
    pdflatex informe.tex
    cd ..
}

compile_slides() {
    echo "Compilando las slides..."
    cd Presentacion
    pdflatex presentacion.tex
    pdflatex presentacion.tex
    pdflatex presentacion.tex
    cd ..
}

show_report() {
    if [ ! -f "informe.pdf" ]; then
        compile_report
    fi

    echo "Mostrando el informe..."
    xdg-open Informe/informe.pdf
}

show_slides() {
    if [ ! -f "diapositivas.pdf" ]; then
        compile_slides
    fi

    echo "Mostrando las diapositivas..."
    xdg-open Presentacion/presentacion.pdf
}

clean() {
    echo "Limpiando archivos auxiliares..."
    cd Informe
    find . -type f ( -iname "*.aux" -o -iname "*.log" -o -iname "*.toc" ) -delete
    cd ..
    cd Presentacion
    find . -type f ( -iname "*.aux" -o -iname "*.log" -o -iname "*.toc" ) -delete
    cd ..
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
        echo "Uso: proyecto.sh [opción]"
        echo "Opciones disponibles:"
        echo "run          - Ejecutar el proyecto"
        echo "report       - Compilar y generar el PDF del informe"
        echo "slides       - Compilar y generar el PDF de las diapositivas"
        echo "show_report  - Mostrar el informe en un visor de PDFs"
        echo "show_slides  - Mostrar las diapositivas en un visor de PDFs"
        echo "clean        - Eliminar archivos auxiliares"
        ;;
esac
