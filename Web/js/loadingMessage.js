let _timer;
const _generatingMessage = 'Generating image';

const loadingMessage = {
    clear: () => {
        document.getElementById('loading').innerHTML = '';
    },

    show: (message) => {
        document.getElementById('loading').innerHTML = message;
    },

    showError: (message) => {
        document.getElementById('loading').innerHTML = '<div class="error">' + message + '</div>';
    },

    stop: () => clearInterval(_timer),

    _tick: () => {
        let x = document.getElementById('loading');

        if (x.innerHTML.endsWith('......')) {
            x.innerHTML = _generatingMessage;
        } else {
            x.innerHTML += '.';
        }
    }
}

loadingMessage.start = () => {
    loadingMessage.show(_generatingMessage);
    _timer = setInterval(loadingMessage._tick, 300);
}