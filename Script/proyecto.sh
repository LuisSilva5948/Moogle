#!/bin/bash

run() {
    echo "Ejecutando el proyecto..."
    cd ..
    dotnet watch run --project MoogleServer
    cd Script
}

report() {
    cd ../Informe
    echo "Compilando y generando el reporte..."
    pdflatex Informe.tex
    cd ../Script
}

slides() {
    cd ../Presentación
    echo "Compilando y generando la presentación..."
    pdflatex Presentación.tex
    cd ../Script
}

show_report() {
    cd ../Informe
    if [ ! -f Informe.pdf ]; then
        report
        cd ../Informe
    fi
    
    if [ $# -eq 0 ]; then
        viewer_command="evince"
    else
        viewer_command=$1
    fi
    
    echo "Mostrando el pdf del informe..."
    $viewer_command Informe.pdf
    cd ../Script
}

show_slides() {
    cd ../Presentación
    
    if [ ! -f Presentación.pdf ]; then
        slides
        cd ../Presentación
    fi
    
    if [ $# -eq 0 ]; then
        viewer_command="evince"
    else
        viewer_command=$1
    fi
    echo "Mostrando el pdf de la presentación..."
    $viewer_command Presentación.pdf
    cd ../Script
}

clean() {
    echo "Eliminando archivos auxiliares..."
    cd ../Informe
    rm -f Informe.aux Informe.fls Informe.log Informe.fdb_latexmk Informe.out Informe.pdf Informe.synctex.gz Informe.toc
    cd ../Presentación
    rm -f Presentación.aux Presentación.fls Presentación.log Presentación.nav Presentación.out Presentación.pdf Presentación.synctex.gz Presentación.snm Presentación.toc Presentación.fdb_latexmk
    cd ../Script
}


case "$1" in
    run)
        run
        ;;
    report)
        report
        ;;
    slides)
        slides
        ;;
    show_report)
        shift
        show_report
        ;;
    show_slides)
        shift
        show_slides
        ;;
    clean)
        clean
        ;;
    *)
        echo "Comando no soportado"
        echo "Uso: sh proyecto.sh {run|report|slides|show_report|show_slides|clean}"
        exit 1
        ;;
esac
