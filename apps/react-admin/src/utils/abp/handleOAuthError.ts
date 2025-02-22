interface OAuthError {
	error: string;
	error_description?: string;
	error_uri?: string;
}

export function handleOAuthError() {
	function formatError(error: OAuthError) {
		// TODO: 解决oauth消息国际化.
		return error.error_description;
	}

	return {
		formatError,
	};
}
