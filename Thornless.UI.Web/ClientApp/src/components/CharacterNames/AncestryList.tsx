import * as React from 'react';
import { connect } from 'react-redux';
import { SanitizedHtml } from '../../shared';
import { ApplicationState } from '../../store';
import * as AncestryStore from '../../store/characterNameService';

import './AncestryList.scss';

// At runtime, Redux will merge together...
type AncestryProps =
  AncestryStore.CharacterNameState
  & typeof AncestryStore.actionCreators


class FetchData extends React.Component<AncestryProps> {
  // This method is called when the component is first added to the document
  public componentDidMount() {
    this.ensureDataFetched();
  }

  // This method is called when the route parameters change
  public componentDidUpdate() {
    this.ensureDataFetched();
  }

  public ancestrySelected(event: React.FormEvent<HTMLButtonElement>) {
    var selectedValue = event.currentTarget.value;
    this.props.setSelectedAncestry(selectedValue);
    this.props.requestAncestryOptions();
  }

  public optionSelected(event: React.FormEvent<HTMLButtonElement>) {
    var selectedOption = event.currentTarget.value;
    this.props.setSelectedOption(selectedOption);
    this.props.requestCharacterNames();
  }

  public render() {
    return (
      <React.Fragment>
        <div id="ancestry-row">
          <div className="col-lg-4 col-12 col-content">{this.renderAncestyDropdown()}</div>
          <div className="col-lg-4 col-12 col-content">{this.renderOptionsDropdown()}</div>
          <div className="col-lg-4 col-12 col-content">{this.renderName()}</div>
        </div>
        {/* {this.renderCountDropdown()}
        {this.renderSubmitButton()} */}
      </React.Fragment>
    );
  }

  private ensureDataFetched() {
    this.props.requestAncestries();
  }

  private renderAncestyDropdown() {
    return (
      <div>
        <h2>Ancestry</h2>
        {this.props?.ancestries.sort((a, b) => a.sortOrder - b.sortOrder).map((ancestry: AncestryStore.Ancestry) =>
          <div className="select-button">
            <button value={ancestry.code} onClick={e => this.ancestrySelected(e)}>{ancestry.name}</button>
          </div>
        )}
      </div>
    );
  }

  private renderOptionsDropdown() {
    return (
      <div>
        <h2>Option</h2>
        <div>
          <h3>{this.props?.ancestryOptions?.name}</h3>
        </div>
        {this.props?.ancestryOptions?.options.sort((a, b) => a.sortOrder - b.sortOrder).map((option: AncestryStore.NameCodeSort) =>
          <div className="select-button">
            <button value={option.code} onClick={e => this.optionSelected(e)}>{option.name}</button>
          </div>
        )}
        <div>
          <p>{this.props?.ancestryOptions?.copyright}</p>
        </div>
      </div>
    );
  }

  private renderName() {
    return (
      <div>
        <h2>Names</h2>
        {this.props?.characterNames.map((name: AncestryStore.CharacterName) =>
          <div>
            <h3>{name.name}</h3>
            <h4>{name.ancestryName} | {name.optionName}</h4>
            <div>
              {name.definitions.map((definition: AncestryStore.CharacterNameDefinition) =>
                <div><b>{definition.namePart}:</b> {definition.meanings.join(", ")}</div>
              )}
            </div>
          </div>
        )}
      </div>
    );
  }
}

export default connect(
  (state: ApplicationState) => state.characterNames, // Selects which state properties are merged into the component's props
  AncestryStore.actionCreators // Selects which action creators are merged into the component's props
)(FetchData as any);
