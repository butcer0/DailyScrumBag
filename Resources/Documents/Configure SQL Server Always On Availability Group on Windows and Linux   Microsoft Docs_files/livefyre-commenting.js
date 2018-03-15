(function LiveFyre() {
	function loadLiveFyreAndOpenCommenting() {
		var livefyre = "//cdn.livefyre.com/Livefyre.js";
		var opencomment = "/_chrome/commenting/open-commenting.entry.min.js";
		return Promise.all([msDocs.functions.loadLibrary(livefyre), msDocs.functions.loadLibrary(opencomment)]);
	}

	loadLiveFyreAndOpenCommenting().then(function () {
		// livefyre adds elements to the DOM before the "https://cdn.livefyre.com/libs/sidenotes/v1.6.0/css/block.css" styles have downloaded
		// hide all livefyre content until after their styles have been applied.
		var hideLiveFyreStyle = document.createElement('style');
		hideLiveFyreStyle.textContent = '.lf { display: none!important; }';
		document.head.appendChild(hideLiveFyreStyle);

		//open-commenting configurations.
		var networkName = "microsoft-int-0.fyre.co", siteId = "306034";
		if (location.hostname === "docs.microsoft.com") {
			//Connect to open-commenting prod network when it's docs production environment.
			networkName = "microsoft.fyre.co";
			siteId = "381623";
		}
		var commentOptions = {
			serviceProvider: "livefyre",
			network: networkName,
			siteId: siteId,
			containerElementId: "comments-container",
			//USER STORY 558932: Use document_id as unique identifier for DOCS commenting and side notes
			documentIdMetaName: "document_id",
			/* open-commenting side-note provides selectors option to find content on the page.
			   It can be a selector string (such as '#content p, #content img'), a jQuery object (such as $('#content')),
			   an array of DOM elements,or an object with two properties: include and exclude. */
			sidenoteSelectors: {
				include: ".content p, .content ul, .content ol",
				exclude: "div.button p, div.step-by-step p, li p, .metadata ul, ul ol, ol ul, ul ul, ol ol, #comments-container p, #comments-container ul, #comments-container ol"
			},
			localeMetaName: "locale", // content locale
			theme: "docs",
			commentCallback: function (data) {
				if (!data.statusCode) {
					data.valueObject.on('initialRenderComplete', function () {
						msDocs.functions.notifyContentUpdated();
						setTimeout(msDocs.functions.notifyContentUpdated, 200);
						setTimeout(function () { document.head.removeChild(hideLiveFyreStyle); }, 1000);
					});

					data.valueObject.on("commentPosted", function (data) {
						msDocs.data.jsllReady.then(function (awa) {
							awa.ct.captureContentPageAction({
								content: {
									type: 'livefyre-comment',
									id: data.id,
									// authorId: data.authorId,
									bodyHtml: data.bodyHtml
								}
							});
						});
					});
				}
			},
			sidenoteCallback: function (data) {
				if (!data.statusCode) {
					data.valueObject.on("sidenotes.commentPosted", function (data) {
						msDocs.data.jsllReady.then(function (awa) {
							awa.ct.captureContentPageAction({
								content: {
									type: 'livefyre-sidenote',
									id: data.id,
									// authorId: data.authorId,
									bodyHtml: data.bodyHtml
								}
							});
						});
					});
				}
			},
			identityServiceCallback: function (data) {
				if (data.statusCode) {
					throw new Error(data.valueObject.message);
				}
			}
		};

		// load commenting widget
		opencomment(commentOptions);
	});
})();
