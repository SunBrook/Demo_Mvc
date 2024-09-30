/**********        瀑布流处理       ***********/
document.addEventListener('DOMContentLoaded', function() {
    const grid = document.querySelector('.grid');
    var msnry;

    // 监听所有图像加载完成
    imagesLoaded(grid, function() {
        // 初始化 Masonry 布局
        msnry = new Masonry(grid, {
            itemSelector: '.grid-item',
            columnWidth: '.grid-item',
            percentPosition: true,
            gutter:20
        });
    });
});

// 使用 imagesLoaded 库监听图像加载
function imagesLoaded(parentNode, callback) {
    var images = parentNode.querySelectorAll('img');
    var count = images.length;
    var loaded = 0;

    function checkDone() {
        loaded++;
        if (loaded === count) {
            callback();
        }
    }

    images.forEach(function(img) {
        if (img.complete) {
            checkDone();
        } else {
            img.addEventListener('load', checkDone);
            img.addEventListener('error', checkDone);
        }
    });
}
