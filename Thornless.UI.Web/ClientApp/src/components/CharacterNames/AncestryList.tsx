import * as React from 'react';
import { connect } from 'react-redux';
import { ApplicationState } from '../../store';
import * as AncestryStore from '../../store/characterNameService';

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

  public ancestrySelected(event: React.FormEvent<HTMLSelectElement>) {
    var selectedValue = event.currentTarget.value;
    this.props.setSelectedAncestry(selectedValue);
    this.props.requestAncestryOptions();
  }

  public optionSelected(event: React.FormEvent<HTMLSelectElement>) {
    var selectedOption = event.currentTarget.value;
    this.props.setSelectedOption(selectedOption);
  }

  public requestCharacterNames(event: React.FormEvent<HTMLButtonElement>) {
    this.props.requestCharacterNames();
  }

  public render() {
    return (
      <React.Fragment>
        {this.renderAncestyDropdown()}
        {this.renderOptionsDropdown()}
        {this.renderSubmitButton()}
        {this.renderName()}
      </React.Fragment>
    );
  }

  private ensureDataFetched() {
    this.props.requestAncestries();
  }

  private renderAncestyDropdown() {
    return (
      <div>
        <select onChange={ e => this.ancestrySelected(e) }>
          { this.props?.ancestries.sort((a, b) => a.sortOrder - b.sortOrder).map((ancestry: AncestryStore.Ancestry) =>
            <option key={ancestry.code} value={ancestry.code}>{ancestry.name}</option>
          )}
        </select>
      </div>
    );
  }

  private renderOptionsDropdown() {
    return (
      <div>
        <select onChange={ e => this.optionSelected(e) }>
          { this.props?.ancestryOptions?.options.sort((a, b) => a.sortOrder - b.sortOrder).map((option: AncestryStore.NameCodeSort) =>
            <option key={option.code} value={option.code}>{option.name}</option>
          )}
        </select>
      </div>
    );
  }

  private renderSubmitButton() {
    return (
      <div>
        <button type="button" onClick={e => this.requestCharacterNames(e) }>Generate Name</button>
      </div>
    );
  }

  private renderName() {
    return (
      <div>
        { this.props?.characterNames.map((name : AncestryStore.CharacterName) =>
          <div>
            <h3>{name.name}</h3>
            <h4>{name.ancestryName} | {name.optionName}</h4>
            <div>
              { name.definitions.map((definition : AncestryStore.CharacterNameDefinition) =>
                <div><b>{ definition.namePart}:</b> { definition.meanings.join(", ") }</div>
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
