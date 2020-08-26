import React, { CSSProperties } from 'react';
import sanitizeHTML from 'sanitize-html';

type ReactSanitizedHtmlProps = 
{
    html: string,
    allowedTags?: string[] | undefined,
    className?: string | undefined,
    id?: string | undefined,
    style?: CSSProperties | undefined
}

class ReactSanitizedHtml extends React.Component<ReactSanitizedHtmlProps>
{
    public render() {
        console.log(JSON.stringify(this.props));
        const sanitizedHTML = sanitizeHTML(
          this.props.html,
          {
            allowedTags: this.props.allowedTags ?? []
          }
        );

        return (
            <React.Fragment>
                <div
                    className={ this.props.className }
                    dangerouslySetInnerHTML={{ __html: sanitizedHTML }}
                    id={ this.props.id }
                    style={ this.props.style }
                    />
            </React.Fragment>
        )
    }
}

export default ReactSanitizedHtml;