class Pagination {
    constructor() {
        this.currentPage = 1;
        this.perPage = 20;
        this.buttonsOffset = 3;
        this.array = [];
    }

    getHtml() {
        let html = "";

        html += "<a href='#' class='button' onclick='first(); return false;'>First</a>";
        html += "<a href='#' class='button' onclick='previus(); return false;'>Previus</a>";

        let pageCount = Math.ceil(this.array.length / this.perPage);

        let pageOffsetLeft = this.currentPage - this.buttonsOffset;
        let pageOffsetRight = pageCount - this.currentPage - this.buttonsOffset;

        if (pageOffsetRight > 0)
            pageOffsetRight = 0;

        if (pageOffsetLeft < 1)
            pageOffsetRight -= pageOffsetLeft - 1;

        if (pageOffsetLeft < 1)
            pageOffsetLeft = 1;

        if (pageOffsetRight < 0)
            pageOffsetLeft += pageOffsetRight;

        let i_ = pageOffsetLeft;
        if (i_ < 1)
            i_ = 1;

        for (let i = i_; i < this.currentPage; i++)
            html += "<a href='#' class='button' onclick='page(" + i + "); return false;'>" + i + "</a>";

        html += "<span class='button current-page'>" + this.currentPage + "</span>";

        for (let i = this.currentPage + 1; i < this.currentPage + 1 + pageOffsetRight + this.buttonsOffset && i <= pageCount; i++)
            html += "<a href='#' class='button' onclick='page(" + i + "); return false;'>" + i + "</a>";

        html += "<a href='#' class='button' onclick='next(); return false;'>Next</a>";
        html += "<a href='#' class='button' onclick='last(); return false;'>Last</a>";

        return html;
    }

    getElementsByPage() {
        let result = [];

        for (let i = (this.currentPage - 1) * this.perPage; i < this.array.length && i < (this.currentPage - 1) * this.perPage + this.perPage; i++)
            result.push(this.array[i]);

        return result;
    }
}