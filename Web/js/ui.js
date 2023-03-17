const useSample = () => {
    var thisYear = new Date().getFullYear();
    const sample = {
        'title': 'Product delivery roadmap',
        'team': 'Your Team',
        'start_date': '01 Jan ' + thisYear.toString(),
        'quarters': 4,
		'debug': false,
		'bg_color_hex': "#ffffff",
        'projects': [
            {
                'name': 'Build Product',
                'label': 'Ongoing',
                'date': '01 Jan ' + thisYear.toString()
            },
            {
                'name': 'Test Product',
                'label': 'Not Started',
                'date': '01 Jun ' + thisYear.toString()
            }
        ]
    };
    document.getElementById('inputText').value = JSON.stringify(sample, null, 4);
    loadingMessage.clear();
}

function parseInputFile() {
    const fileControl = document.getElementById('inputFile');

    if (!fileControl.files.length) {
        loadingMessage.show('No file selected!');
        alert('No file selected.');
        return;
    }

    const inputFile = document.getElementById('inputFile').files[0];

    const fileReader = new FileReader();
    fileReader.onload = onFileReaderLoad;
    fileReader.readAsText(inputFile);
}

function onFileReaderLoad(event) {
    document.getElementById('inputText').value = event.target.result;

    validateJson(event.target.result);
}

function validateJson(jsonString) {
    loadingMessage.clear();

    if (!jsonString) {
        loadingMessage.showError('Need JSON to generate an image!');
        return;
    }

    try {
        return JSON.parse(jsonString);
    }
    catch {
        loadingMessage.showError('Invalid JSON.');
    }
}

function runUsingJsonFromTextArea() {
    run(document.getElementById('inputText').value);
}

function run(jsonString) {
    callAPI(validateJson(jsonString));
}

var callAPI = (inputObject) => {
    loadingMessage.start();
    document.getElementById('resultImage').innerHTML = '';

    const headers = new Headers();
    headers.append('Content-Type', 'application/json');
    headers.append('Authorization', 'Bearer ' + window.token);

    const raw = JSON.stringify(inputObject);

    const requestOptions = {
        method: 'POST',
        headers: headers,
        body: raw,
        redirect: 'follow'
    };

    fetch(window.apiUrl, requestOptions)
    .then(response => {
        loadingMessage.stop();

        if (response.status === 401) {
            loadingMessage.show('Unauthorized.');
        } else {
            return response.text();
        }
    })
    .then(result => {
        const base64 = JSON.parse(result);

        loadingMessage.show('Done. Right-click on image and Save to download it.');
        document.getElementById('resultImage').innerHTML = '<img style="width:100%" src="data:image/png;base64, ' + base64 + '" />';
    })
    .catch(error => {
        loadingMessage.stop();
        console.log('error', error);

        const message = error.toString().indexOf('Failed to fetch') > -1
            ? error + '. Is the API URL correct? Is it running?'
            : error;

        loadingMessage.showError(message);
    });
}

function uploadFile() {
    parseInputFile();
}

function saveToFile() {
    const obj = validateJson(document.getElementById('inputText').value);
    downloadToFile(
        document.getElementById('inputText').value,
        (obj.team || 'roadmap_input') + '.json',
        'application/json'
    );
}

// Borrowed from https://robkendal.co.uk/blog/2020-04-17-saving-text-to-client-side-file-using-vanilla-js
function downloadToFile(content, filename, contentType) {
    const a = document.createElement('a');
    const file = new Blob([content], {type: contentType});

    a.href= URL.createObjectURL(file);
    a.download = filename;
    a.click();

    URL.revokeObjectURL(a.href);
}

document.getElementById('inputText').addEventListener('input', () => loadingMessage.clear());
