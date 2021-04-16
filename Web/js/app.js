let auth0 = null;

const fetchAuthConfig = () => fetch('/auth_config.json');

const configureClient = async () => {
    const response = await fetchAuthConfig();
    const config = await response.json();

    const companySpans = document.getElementsByClassName('companyName');
    for (var i = 0; i < companySpans.length; i++) {
        companySpans[i].innerHTML = config.company;
    }

    window.apiUrl = config.apiUrl;

    document.getElementById('authenticating').innerHTML = (window.apiUrl.indexOf('<') > -1)
        ? 'Fill out auth_config.json with the correct values, then refresh.'
        : 'Verifying authentication for ' + window.apiUrl + '...';

    auth0 = await createAuth0Client({
        domain: config.domain,
        client_id: config.clientId,
        audience: config.audience
    });
};

window.onload = async () => {
    await configureClient();

    updateUI();

    const isAuthenticated = await auth0.isAuthenticated();

    if (isAuthenticated) {
        return;
    }

    // NEW - check for the code and state parameters
    const query = window.location.search;
    if (query.includes('code=') && query.includes('state=')) {
        // Process the login state
        await auth0.handleRedirectCallback();

        updateUI();

        // Use replaceState to redirect the user away and remove the querystring parameters
        window.history.replaceState({}, document.title, '/');
    }
}

const updateUI = async () => {
    document.getElementById('authenticating').classList.add('hidden');

    const isAuthenticated = await auth0.isAuthenticated();

    if (isAuthenticated) {
        document.getElementById('public-content').classList.add('hidden');
        document.getElementById('instructions').classList.add('hidden');
        document.getElementById('btn-logout').disabled = false;
        document.getElementById('btn-login').disabled = true;
        document.getElementById('btn-logout').classList.remove('hidden');
        document.getElementById('btn-login').classList.add('hidden');

        document.getElementById('gated-content').classList.remove('hidden');

        const token = await auth0.getTokenSilently();
        window.token = token;
    
        const user = await auth0.getUser();

        const myProfile =
            '<div style="display: flex; flex-direction: row">' +
                '<div style="margin-right: 3px"><img src="' + user.picture + '" width="35px" style="border:1px solid black;border-radius:4px" /></div>' +
                '<div><div>' + user.name + '</div><div>' + user.email + '</div></div>' +
            '</div>';
        document.getElementById('my-profile').innerHTML = myProfile;

        useSample();
      } else {
        document.getElementById('instructions').classList.remove('hidden');
        document.getElementById('public-content').classList.remove('hidden');
        document.getElementById('btn-logout').disabled = true;
        document.getElementById('btn-login').disabled = false;
        document.getElementById('btn-logout').classList.add('hidden');
        document.getElementById('btn-login').classList.remove('hidden');
        document.getElementById('gated-content').classList.add('hidden');
      }
};

const login = async () => {
    await auth0.loginWithRedirect({
        redirect_uri: window.location.origin
    })
};

const logout = () => {
    auth0.logout({
        returnTo: window.location.origin
    });
};

function useSample() {
    var thisYear = new Date().getFullYear();
    const sample = {
        'team': 'Your Team',
        'start_date': '01/01/' + thisYear.toString(),
        'projects': [
            {
                'name': 'Build Product',
                'label': 'Ongoing',
                'date': '01/01/' + thisYear.toString()
            },
            {
                'name': 'Test Product',
                'label': 'Not Started',
                'date': '06/01/' + thisYear.toString()
            }
        ]
    };
    document.getElementById('inputText').value = JSON.stringify(sample, null, 4)
}
